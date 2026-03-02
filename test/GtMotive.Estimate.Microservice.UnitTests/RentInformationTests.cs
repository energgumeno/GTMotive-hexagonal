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
        rentInfo.Accept();

        // Assert
        Assert.Equal(RentStatus.Accepted, rentInfo.Status);
    }

    [Fact]
    public void Accept_WhenStatusIsNotNew_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var rentInfo = RentInformation.Create("John", "john@example.com", DateTime.Now, DateTime.Now.AddDays(1),
            Guid.NewGuid());
        rentInfo.Accept(); // Now status is Accepted

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => rentInfo.Accept());
    }
}