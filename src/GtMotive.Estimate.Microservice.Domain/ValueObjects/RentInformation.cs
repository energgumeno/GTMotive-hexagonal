using System.Text.Json.Serialization;
using GtMotive.Estimate.Microservice.Domain.Common;
using GtMotive.Estimate.Microservice.Domain.Enums;

namespace GtMotive.Estimate.Microservice.Domain.ValueObjects;

public class RentInformation : BaseAggregate
{
    private const string TimeRentStartBiggerTimeRentEnd = "TimeRentStart must be less than TimeRentEnd";
    private const string CannotCancelRentIfStatusIsNotNew = "Cannot cancel rent if status of current rent is not New";
    private const string CannotReturnifNotAccepted = "Cannot return vehicle if rent is not accepted";
    private const string VehicleNotAvailableTime = "The vehicle is not available in that Time Range.";

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

    public void Confirm()
    {
        if (Status != RentStatus.New)
            throw new InvalidOperationException(CannotCancelRentIfStatusIsNotNew);
        Status = RentStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status == RentStatus.New)
            throw new InvalidOperationException(CannotReturnifNotAccepted);
        Status = RentStatus.Cancelled;
    }

    public void ReturnVehicle()
    {
        if (Status != RentStatus.Confirmed && Status != RentStatus.Cancelled)
            throw new InvalidOperationException();
        Status = RentStatus.Returned;
    }

    public void ValidateFinishedLease()
    {
        if (Status is not (RentStatus.Cancelled or RentStatus.Returned))
        {
            throw new InvalidOperationException($"{Email} has already a Lease.");
        }
    }

    public bool IsTimeAvailable(DateTime newTimeRentStart, DateTime newTimeRentEnd)
    {
        bool startBetweenTimeStartAndEnd = newTimeRentStart > TimeRentStart && newTimeRentStart < TimeRentEnd;
        bool endBetweenTimeStartAndEnd = newTimeRentEnd > TimeRentStart && newTimeRentEnd < TimeRentEnd;
        bool timeIncludesRent = newTimeRentStart < TimeRentStart && newTimeRentEnd > TimeRentEnd;
        return !startBetweenTimeStartAndEnd && !endBetweenTimeStartAndEnd && !timeIncludesRent;
    }

    public void ValidateVehicleAvailability(DateTime newTimeRentStart, DateTime newTimeRentEnd)
    {
        if (!IsTimeAvailable(newTimeRentStart, newTimeRentEnd))
        {
            throw new InvalidOperationException(VehicleNotAvailableTime);
        }
    }

    public bool IsConflict(RentInformation newRent)
    {
        return VehicleId == newRent.VehicleId
               &&
               Id != newRent.Id
               &&
               Status == RentStatus.Confirmed
               &&
               IsTimeAvailable(newRent.TimeRentStart, newRent.TimeRentEnd);
    }

    protected bool Equals(RentInformation other)
    {
        return Fullname == other.Fullname && Email == other.Email && VehicleId.Equals(other.VehicleId) && Status == other.Status && TimeRentStart.Equals(other.TimeRentStart) && TimeRentEnd.Equals(other.TimeRentEnd);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((RentInformation)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Fullname, Email, VehicleId, (int)Status, TimeRentStart, TimeRentEnd);
    }
}