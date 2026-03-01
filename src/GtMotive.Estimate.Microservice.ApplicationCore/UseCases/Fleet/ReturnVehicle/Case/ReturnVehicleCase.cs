using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.ReturnVehicle.Commands;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.ReturnVehicle.Case
{
    public class ReturnVehicleCase : IUseCase<ReturnVehicleCommand>
    {
        public Task Execute(ReturnVehicleCommand request)
        {
            throw new NotImplementedException();
        }
    }
}
