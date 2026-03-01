using System;
using GtMotive.Estimate.Microservice.Domain.Entities;

namespace GtMotive.Estimate.Microservice.Domain.Aggregates
{
    public class RentInformation : BaseAggregate
    {
        //should be moved to user in a bounded context
        public string Fullname { get; set; }
        public string Email { get; set; }

        //should be moved to user in a bounded context

        public Guid VehicleId { get; set; }
        public RentStatus Status { get; set; }
        public DateTime TimeRentStart { get; set; }
        public DateTime TimeRentEnd { get; set; }

        public RentInformation(
            string fullname,
            string? email,
            DateTime? timeRentStart,
            DateTime? timeRentEnd,
            Guid? vehicleId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(fullname);
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            if (!timeRentStart.HasValue) throw new ArgumentNullException(nameof(timeRentStart));
            if (!timeRentEnd.HasValue) throw new ArgumentNullException(nameof(timeRentEnd));
            if (!vehicleId.HasValue) throw new ArgumentNullException(nameof(vehicleId));

            Fullname = fullname;
            Email = email;
            TimeRentStart = timeRentStart ?? DateTime.Now;
            TimeRentEnd = timeRentEnd ?? DateTime.Now;
            VehicleId = vehicleId ?? Guid.NewGuid();
            Status = RentStatus.New;
        }
    }
}