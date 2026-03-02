using GtMotive.Estimate.Microservice.Domain.Common;
using GtMotive.Estimate.Microservice.Domain.Enums;

namespace GtMotive.Estimate.Microservice.Domain.ValueObjects;

public class RentInformation : BaseAggregate
{
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
        VehicleId = vehicleId ?? Guid.Empty;
        Status = RentStatus.New;
    }

    //should be moved to user in a bounded context
    public string Fullname { get; private set; }
    public string Email { get; private set; }

    //should be moved to user in a bounded context

    public Guid VehicleId { get; private set; }
    public RentStatus Status { get; private set; }
    public DateTime TimeRentStart { get; private set; }
    public DateTime TimeRentEnd { get; private set; }


    public static RentInformation Create(
        string fullname,
        string? email,
        DateTime? timeRentStart,
        DateTime? timeRentEnd,
        Guid? vehicleId)
    {
        return new RentInformation(
            fullname,
            email,
            timeRentStart,
            timeRentEnd,
            vehicleId)
        {
            Id = Guid.NewGuid()
        };
    }

    public void Accept()
    {
        if (Status != RentStatus.New)
            throw new InvalidOperationException("Cannot accept rent if status is not New");
        Status = RentStatus.Accepted;
    }

    public void Cancel()
    {
        if (Status != RentStatus.New)
            throw new InvalidOperationException("Cannot cancel rent if status is not New");
        Status = RentStatus.Cancelled;
    }

    public void ReturnVehicle()
    {
        if (Status != RentStatus.Accepted)
            throw new InvalidOperationException("Cannot return vehicle if rent is not accepted");
        Status = RentStatus.Returned;
    }
}