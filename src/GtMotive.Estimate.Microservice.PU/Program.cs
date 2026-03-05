using GtMotive.Estimate.Microservice.ApplicationCore.DependencyInjection;
using GtMotive.Estimate.Microservice.Infrastructure.DependencyInjection;
using GtMotive.Estimate.Microservice.PU.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationCore();

// Handlers
builder.Services.AddScoped<IMessageHandler, RentVehicleCreatedMessageHandler>();
builder.Services.AddScoped<IMessageHandler, RentVehicleReturnedMessageHandler>();

builder.Services.AddHostedService<ServiceBusReceiverWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();


app.Run();