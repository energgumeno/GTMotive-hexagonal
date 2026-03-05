using GtMotive.Estimate.Microservice.Domain.Enums;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.UnitTests;

public class RentInformationTests
{
    [Fact]
    public void Accept_WhenStatusIsNew_ShouldChangeToAccepted()
    {
        // Arrange
        var fullname = "John Doe";
        var email = "john@example.com";
        var start = DateTime.Now.AddDays(1);
        var end = DateTime.Now.AddDays(2);
        var vehicleId = Guid.NewGuid();

        var rentInfo = RentInformation.Create(fullname, email, start, end, vehicleId);

        // Act
        rentInfo.Confirm();

        // Assert
        Assert.Equal(RentStatus.Confirmed, rentInfo.Status);
    }

    [Fact]
    public void Accept_WhenStatusIsNotNew_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var rentInfo = RentInformation.Create("John", "john@example.com", DateTime.Now, DateTime.Now.AddDays(1),
            Guid.NewGuid());
        rentInfo.Confirm(); // Now status is Accepted

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => rentInfo.Confirm());
    }

    [Fact]
    public void IsConflict_ShouldDetectConflictsCorrecty()
    {
        var vehicleId = Guid.NewGuid();
        var start = new DateTime(2026, 3, 5, 10, 0, 0);
        var end = new DateTime(2026, 3, 5, 12, 0, 0);

        var existingRent = RentInformation.Create("User 1", "user1@example.com", start, end, vehicleId);
        existingRent.Confirm();

        // Conflict: Same vehicle, same time
        var newRent1 = RentInformation.Create("User 2", "user2@example.com", start, end, vehicleId);
        Assert.True(existingRent.IsConflict(newRent1));

        // Conflict: Same vehicle, overlapping (start inside)
        var newRent2 = RentInformation.Create("User 2", "user2@example.com", start.AddHours(-1), start.AddHours(1),
            vehicleId);
        Assert.True(existingRent.IsConflict(newRent2));

        // Conflict: Same vehicle, overlapping (end inside)
        var newRent3 =
            RentInformation.Create("User 2", "user2@example.com", end.AddHours(-1), end.AddHours(1), vehicleId);
        Assert.True(existingRent.IsConflict(newRent3));

        // Conflict: Same vehicle, new rent includes existing rent
        var newRent4 = RentInformation.Create("User 2", "user2@example.com", start.AddHours(-1), end.AddHours(1),
            vehicleId);
        Assert.True(existingRent.IsConflict(newRent4));

        // No conflict: Different vehicle
        var newRent5 = RentInformation.Create("User 2", "user2@example.com", start, end, Guid.NewGuid());
        Assert.False(existingRent.IsConflict(newRent5));

        // No conflict: Same vehicle, after existing rent
        var newRent6 =
            RentInformation.Create("User 2", "user2@example.com", end.AddHours(1), end.AddHours(2), vehicleId);
        Assert.False(existingRent.IsConflict(newRent6));

        // No conflict: Same vehicle, before existing rent
        var newRent7 = RentInformation.Create("User 2", "user2@example.com", start.AddHours(-2), start.AddHours(-1),
            vehicleId);
        Assert.False(existingRent.IsConflict(newRent7));

        // No conflict: Same rent ID
        Assert.False(existingRent.IsConflict(existingRent));
    }
}