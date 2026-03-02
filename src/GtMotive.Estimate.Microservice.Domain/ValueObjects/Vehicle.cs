using System.Text.Json.Serialization;
using GtMotive.Estimate.Microservice.Domain.Common;

namespace GtMotive.Estimate.Microservice.Domain.ValueObjects;

/// <summary>
///     A vehicle.
/// </summary>
public class Vehicle : BaseAggregate
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Vehicle" /> class.
    /// </summary>
    /// <param name="registrationDate"> registration date.</param>
    /// <param name="frameId">Frame Id.</param>
    /// <param name="licensePlate">License plate.</param>
    public Vehicle(DateTime? registrationDate, string? frameId, string? licensePlate)
    {
        registrationDate = registrationDate?.Date;

        ArgumentException.ThrowIfNullOrWhiteSpace(frameId);
        ArgumentException.ThrowIfNullOrWhiteSpace(licensePlate);

        RegistrationDate = registrationDate ?? DateTime.MinValue;

        if (RegistrationDate.AddYears(5) < DateTime.Today)
            throw new ArgumentException("Registration date should not be older than 5 years");

        FrameId = frameId;
        LicensePlate = licensePlate;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Vehicle" /> class.
    ///     This constructor is for deserialization only.
    /// </summary>
    public Vehicle()
    {
    }

    /// <summary>
    ///     Gets Registration date.
    /// </summary>
    [JsonInclude]
    public DateTime RegistrationDate { get; private set; }

    /// <summary>
    ///     Gets Frame id.
    /// </summary>
    [JsonInclude]
    public string? FrameId { get; private set; }

    /// <summary>
    ///     Gets License Plate.
    /// </summary>
    [JsonInclude]
    public string? LicensePlate { get; private set; }

    /// <summary>
    ///     Factory, creates vehicle.
    /// </summary>
    /// <param name="registrationDate"> the registration date.</param>
    /// <param name="frameId"> the frame id.</param>
    /// <param name="licensePlate"> the license plate.</param>
    /// <returns> a vehicle.</returns>
    public static Vehicle Create(DateTime? registrationDate, string? frameId, string? licensePlate)
    {
        var vehicle = new Vehicle(registrationDate, frameId, licensePlate)
        {
            Id = Guid.NewGuid()
        };

        return vehicle;
    }
}