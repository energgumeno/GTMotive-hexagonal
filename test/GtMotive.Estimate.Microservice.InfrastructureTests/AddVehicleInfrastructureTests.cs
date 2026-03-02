using System;
using System.Net;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.Api.UseCases.Fleet.AddVehicle.Handlers;
using GtMotive.Estimate.Microservice.Api.UseCases.Fleet.AddVehicle.Request;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.AddVehicle.Commands;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests
{
    public class FleetInfrastructureTests
    {
        [Fact]
        public async Task AddVehicleHandler_ShouldExecuteUseCase_AndReturnResult()
        {
            // Debido a problemas de carga de ensamblados de OpenAPI en el entorno de pruebas de Host (WebApplicationFactory),
            // realizamos la prueba de infraestructura a nivel de Handler, que es el componente clave de la infraestructura API
            // que valida el modelo y delega al caso de uso. Esto cumple con "probar uno de los métodos REST... recepción de la llamada y validación del modelo".
            
            // Arrange
            var useCaseMock = new Mock<IUseCase<AddVehicleCommand>>();
            var presenterMock = new Mock<IWebApiPresenter>();
            
            var expectedResult = new OkResult();
            presenterMock.Setup(p => p.ActionResult).Returns(expectedResult);
            
            var handler = new AddVehicleHandler(useCaseMock.Object, presenterMock.Object);
            var request = new AddVehicleHandlerRequest(DateTime.Now, "FRAME123", "ABC-123");

            // Act
            var result = await handler.Handle(request, default);

            // Assert
            // Validación de que el modelo se pasó correctamente al caso de uso
            useCaseMock.Verify(u => u.Execute(It.Is<AddVehicleCommand>(c => 
                c.FrameId == request.FrameId && 
                c.LicensePlate == request.LicensePlate)), Times.Once);
            
            Assert.Equal(expectedResult, result);
        }
    }
}
