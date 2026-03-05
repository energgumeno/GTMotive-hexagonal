using GtMotive.Estimate.Microservice.Infrastructure.Outbox;
using MongoDB.Driver;
using Moq;

namespace GtMotive.Estimate.Microservice.InfrastructureTests;

public class OutboxBusTests
{
    private readonly Mock<IMongoCollection<OutboxMessage>> _mongoCollectionMock;
    private readonly Mock<IMongoDatabase> _mongoDatabaseMock;

    public OutboxBusTests()
    {
        _mongoDatabaseMock = new Mock<IMongoDatabase>();
        _mongoCollectionMock = new Mock<IMongoCollection<OutboxMessage>>();

        _mongoDatabaseMock.Setup(d => d.GetCollection<OutboxMessage>("Outbox", null))
            .Returns(_mongoCollectionMock.Object);
    }

    [Fact]
    public async Task Send_ShouldInsertMessageIntoOutboxCollection()
    {
        // Arrange
        var bus = new OutboxBus(_mongoDatabaseMock.Object);
        var message = new { Data = "Test" };

        // Act
        await bus.Send(message);

        // Assert
        _mongoCollectionMock.Verify(c => c.InsertOneAsync(
            It.Is<OutboxMessage>(m => m.Content.Contains("Test") && m.Type.Contains("AnonymousType")),
            null,
            default), Times.Once);
    }

    [Fact]
    public async Task Send_WhenMessageIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var bus = new OutboxBus(_mongoDatabaseMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => bus.Send(null!));
    }
}