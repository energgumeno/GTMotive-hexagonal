using GtMotive.Estimate.Microservice.ApplicationCore.DependencyInjection;
using GtMotive.Estimate.Microservice.Infrastructure.DependencyInjection;
using GtMotive.Estimate.Microservice.PU;
using GtMotive.Estimate.Microservice.PU.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationCore();
builder.Services.AddHostedService<ServiceBusReceiverWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();



app.Run();

