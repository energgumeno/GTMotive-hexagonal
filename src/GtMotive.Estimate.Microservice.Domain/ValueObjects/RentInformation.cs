using System.Text.Json.Serialization;
using GtMotive.Estimate.Microservice.Domain.Common;
using GtMotive.Estimate.Microservice.Domain.Enums;

namespace GtMotive.Estimate.Microservice.Domain.ValueObjects;

public class RentInformation : BaseAggregate
{
    const string TimeRentStartBiggerTimeRentEnd = "TimeRentStart must be less than TimeRentEnd"

    public RentInformation(
        string fullname,
        string? email,
        DateTime? timeRentStart,
        DateTime? timeRentEnd,
        Guid? vehicleId,
        RentStatus status)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullname);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        if (!timeRentStart.HasValue) throw new ArgumentNullException(nameof(timeRentStart));
        if (!timeRentEnd.HasValue) throw new ArgumentNullException(nameof(timeRentEnd));
        if (!vehicleId.HasValue) throw new ArgumentNullException(nameof(vehicleId));
        if (timeRentStart > timeRentEnd) throw new ArgumentException(TimeRentStartBiggerTimeRentEnd);

        Fullname = fullname;
        Email = email;
        TimeRentStart = timeRentStart ?? DateTime.Now;
        TimeRentEnd = timeRentEnd ?? DateTime.Now;
        VehicleId = vehicleId ?? Guid.Empty;
        Status = status;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="RentInformation" /> class.
    ///     This constructor is for deserialization only.
    /// </summary>
    public RentInformation()
    {
    }

    //should be moved to user in a bounded context
    [JsonInclude] public string Fullname { get; private set; }
    [JsonInclude] public string Email { get; private set; }

    //should be moved to user in a bounded context

    [JsonInclude] public Guid VehicleId { get; private set; }
    [JsonInclude] public RentStatus Status { get; private set; }
    [JsonInclude] public DateTime TimeRentStart { get; private set; }
    [JsonInclude] public DateTime TimeRentEnd { get; private set; }


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
            vehicleId,
            RentStatus.New)
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
        if (Status == RentStatus.New)
            throw new InvalidOperationException("Cannot cancel rent if status is not New");
        Status = RentStatus.Cancelled;
    }

    public void ReturnVehicle()
    {
        if (Status != RentStatus.Accepted && Status != RentStatus.Cancelled)
            throw new InvalidOperationException("Cannot return vehicle if rent is not accepted");
        Status = RentStatus.Returned;
    }

    public void ValidateExistingLease(RentInformation rentVehicle)
    {
        if (rentVehicle.Status is not (RentStatus.Cancelled or RentStatus.Returned))
        {
            throw new InvalidOperationException($"{rentVehicle.Email} has already a Lease.");
        }
    }

    public void ValidateVehicleAvailability(DateTime newTimeRentStart, DateTime newTimeRentEnd)
    {

        if (newTimeRentStart <= TimeRentStart &&
            TimeRentStart <= TimeRentEnd)
        {
            throw new InvalidOperationException($"the vehicle is not available in that {nameof(TimeRentStart)}. ");
        }

        if (newTimeRentEnd <= TimeRentEnd &&
            TimeRentStart <= TimeRentEnd)
        {
            throw new InvalidOperationException($"the vehicle is not available in that {nameof(TimeRentEnd)}. ");
        }
    }
}