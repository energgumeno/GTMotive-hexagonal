using System.Linq.Expressions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.RentVehicle.Case;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.RentVehicle.Commands;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Port;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;
using Moq;

namespace GtMotive.Estimate.Microservice.FunctionalTests;

public class RentVehicleFunctionalTests
{
    private readonly Mock<IBusFactory> _busFactoryMock;
    private readonly Mock<IBus> _busMock;
    private readonly Mock<IOutputPortStandard<RentVehicleResponse>> _outputPortStandardMock;
    private readonly Mock<IErrorOutputPort> _errorOutputPortMock;
    private readonly Mock<IAppLogger<RentVehicleCase>> _loggerMock;
    private readonly Mock<IRentVehiclePort> _rentVehiclePortMock;
    private readonly Mock<ITelemetry> _telemetryMock;
    private readonly RentVehicleCase _useCase;
    private readonly Mock<IVehiclePort> _vehiclePortMock;

    public RentVehicleFunctionalTests()
    {
        _vehiclePortMock = new Mock<IVehiclePort>();
        _rentVehiclePortMock = new Mock<IRentVehiclePort>();
        _busFactoryMock = new Mock<IBusFactory>();
        _busMock = new Mock<IBus>();
        _telemetryMock = new Mock<ITelemetry>();
        _outputPortStandardMock = new Mock<IOutputPortStandard<RentVehicleResponse>>();
        _errorOutputPortMock = new Mock<IErrorOutputPort>();
        _loggerMock = new Mock<IAppLogger<RentVehicleCase>>();

        _busFactoryMock.Setup(f => f.GetClient(It.IsAny<Type>())).Returns(_busMock.Object);

        // Integración de UseCase con Lógica de Dominio real (VehicleRentAggregate)
        _useCase = new RentVehicleCase(
            _vehiclePortMock.Object,
            _rentVehiclePortMock.Object,
            _busFactoryMock.Object,
            _telemetryMock.Object,
            _loggerMock.Object,
            _outputPortStandardMock.Object,
            _errorOutputPortMock.Object);
    }

    [Fact]
    public async Task Execute_WhenVehicleExistsAndNoPreviousRent_ShouldSucceed_AndApplyDomainLogic()
    {
        // Arrange
        var vehicle = Vehicle.Create(DateTime.Now.AddYears(-1), "FRAME123", "ABC-123");
        var vehicleId = vehicle.Id;
        var command = new RentVehicleCommand("John Doe", "john@example.com", DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(2), vehicleId);

        _vehiclePortMock.Setup(v => v.GetVehicle(It.IsAny<Expression<Func<Vehicle, bool>>>()))
            .ReturnsAsync(vehicle);
        _rentVehiclePortMock.Setup(r => r.GetVehiclesRent(It.IsAny<Expression<Func<RentInformation, bool>>>()))
            .ReturnsAsync(new List<RentInformation>());

        // Act
        await _useCase.Execute(command);

        // Assert
        // Verificamos que se llamó al puerto con la información correcta procesada por el dominio
        _rentVehiclePortMock.Verify(r => r.AddVehicleRent(It.Is<RentInformation>(ri =>
            ri.Fullname == command.Fullname &&
            ri.Email == command.Email &&
            ri.VehicleId == vehicleId)), Times.Once);

        _outputPortStandardMock.Verify(o => o.StandardHandle(It.IsAny<RentVehicleResponse>()), Times.Once);
        _busMock.Verify(b => b.Send(It.IsAny<IDomainEvent>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Execute_WhenVehicleDoesNotExist_ShouldCallBadRequest()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var command = new RentVehicleCommand("John Doe", "john@example.com", DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(2), vehicleId);

        _vehiclePortMock.Setup(v => v.GetVehicle(veh => veh.Id == vehicleId)).ReturnsAsync((Vehicle?)null);

        // Act
        await _useCase.Execute(command);

        // Assert
        _errorOutputPortMock.Verify(o => o.BadRequestHandle(It.IsAny<string>()), Times.Once);
        _rentVehiclePortMock.Verify(r => r.AddVehicleRent(It.IsAny<RentInformation>()), Times.Never);
    }
}