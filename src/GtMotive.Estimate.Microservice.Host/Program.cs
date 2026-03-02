using GtMotive.Estimate.Microservice.Api.DependencyInjection;
using GtMotive.Estimate.Microservice.Api.UseCases.Fleet.AddVehicle.Handlers;
using GtMotive.Estimate.Microservice.Host;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddPresenters(builder.Configuration);
builder.Services.AddMediatR(typeof(AddVehicleHandler).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

public partial class Program { }
