using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentReturned.Commands;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Port;
using GtMotive.Estimate.Microservice.Domain.ValueObjects.Aggregates;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentReturned.Case;

public class ProcessRentReturnedCase(IRentVehiclePort rentVehiclePort, IAppLogger<ProcessRentReturnedCase> logger) : IUseCase<ProcessRentReturnedCommand>
{
    public async Task Execute(ProcessRentReturnedCommand request)
    {
        try
        {
            var rent = request.Event.RentInformation;

            //delete the rent, move it, send invoice etc.
            
            await rentVehiclePort.Save();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }
}