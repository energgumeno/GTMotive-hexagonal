using System;
using GtMotive.Estimate.Microservice.Domain.Entities;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects
{
    /// <summary>
    /// A vehicle.
    /// </summary>
    public class Vehicle : BaseAggregate
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Vehicle"/> class.
        /// </summary>
        /// <param name="registrationDate"> registration date.</param>
        /// <param name="frameId">Frame Id.</param>
        /// <param name="licensePlate">License plate.</param>
        private Vehicle(DateTime? registrationDate, string? frameId, string? licensePlate)
        {
            registrationDate = registrationDate?.Date;
            
            ArgumentException.ThrowIfNullOrWhiteSpace(frameId);
            ArgumentException.ThrowIfNullOrWhiteSpace(licensePlate);
            
            RegistrationDate = registrationDate ?? DateTime.MinValue;

            if (RegistrationDate.AddYears(5) < DateTime.Today)
            {
                throw new ArgumentException("Registration date should not be older than 5 years");
            }

            FrameId = frameId;
            LicensePlate = licensePlate;
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Gets Registration date.
        /// </summary>
        public DateTime RegistrationDate { get; }

        /// <summary>
        /// Gets Frame id.
        /// </summary>
        public string FrameId { get; }

        /// <summary>
        /// Gets License Plate.
        /// </summary>
        public string LicensePlate { get; }

        /// <summary>
        /// Factory, creates vehicle.
        /// </summary>
        /// <param name="registrationDate"> the registration date.</param>
        /// <param name="frameId"> the frame id.</param>
        /// <param name="licensePlate"> the license plate.</param>
        /// <returns> a vehicle.</returns>
        public static Vehicle Create(DateTime? registrationDate, string? frameId, string? licensePlate)
        {
            return new Vehicle(registrationDate, frameId, licensePlate);
        }
    }
}
