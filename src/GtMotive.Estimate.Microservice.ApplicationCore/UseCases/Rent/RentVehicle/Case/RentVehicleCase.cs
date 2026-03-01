using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.RentVehicle.Commands;
using GtMotive.Estimate.Microservice.Domain.Events;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Port;
using GtMotive.Estimate.Microservice.Domain.ValueObjects.Aggregates;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.RentVehicle.Case
{
    public class RentVehicleCase(
        IVehiclePort vehiclePort,
        IRentVehiclePort rentVehiclePort,
        IBusFactory busFactory,
        ITelemetry telemetry,
        IOutputPortStandard<RentVehicleResponse> outputPortStandard) : IUseCase<RentVehicleCommand>
    {
        public async Task Execute(RentVehicleCommand request)
        {
            
            ArgumentNullException.ThrowIfNull(request);
            ArgumentException.ThrowIfNullOrWhiteSpace(request.Fullname);
            if (!request.TimeRentStart.HasValue) throw new ArgumentNullException(nameof(request.TimeRentStart));
            if (!request.TimeRentEnd.HasValue) throw new ArgumentNullException(nameof(request.TimeRentEnd));
            if (!request.VehicleId.HasValue) throw new ArgumentNullException(nameof(request.VehicleId));
            
            var bus = busFactory.GetClient(typeof(VehicleCreatedEvent));
            using (vehiclePort)
            {
                var vehicle = vehiclePort.GetVehicle(request.VehicleId.Value);

                if (vehicle == null)
                    throw new InvalidOperationException($"Vehicle with id {request.VehicleId.Value} not found");
            }

            var vehicleRentAggregate = VehicleRentAggregate.Create(
                request.Fullname,
                request.Email,
                request.TimeRentStart,
                request.TimeRentEnd,
                request.VehicleId);
            using (rentVehiclePort)
            {
                await rentVehiclePort.AddVehicleRent(vehicleRentAggregate.RentVehicleInformation);
                telemetry.TrackEvent(nameof(RentVehicleCase),
                    new Dictionary<string, string>() { { nameof(RentVehicleCase), "Start..." } });

                foreach (var vehicleRentAggregateDomainEvent in vehicleRentAggregate.DomainEvents)
                {
                    await bus.Send(vehicleRentAggregateDomainEvent);
                }

                telemetry.TrackEvent(nameof(RentVehicleCase),
                    new Dictionary<string, string>() { { nameof(RentVehicleCase), "End..." } });
            }

            outputPortStandard.StandardHandle(new RentVehicleResponse(vehicleRentAggregate.RentVehicleInformation.Id));
        }
    }
}