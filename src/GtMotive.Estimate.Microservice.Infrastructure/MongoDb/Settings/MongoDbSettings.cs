namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;

    public string MongoDbDatabaseName { get; set; } = string.Empty;
}