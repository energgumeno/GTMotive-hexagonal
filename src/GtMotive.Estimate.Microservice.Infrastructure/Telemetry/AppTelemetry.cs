using System.Diagnostics.CodeAnalysis;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using Microsoft.ApplicationInsights;

namespace GtMotive.Estimate.Microservice.Infrastructure.Telemetry;

[ExcludeFromCodeCoverage]
public class AppTelemetry : ITelemetry
{
    private readonly TelemetryClient _telemetryClient;

    public AppTelemetry(TelemetryClient telemetry)
    {
        _telemetryClient = telemetry;
    }

    public void TrackEvent(string eventName, IDictionary<string, string>? properties = null)
    {
        _telemetryClient.TrackEvent(eventName, properties);
    }

    public void TrackMetric(string name, double value, IDictionary<string, string>? properties = null)
    {
        _telemetryClient.TrackMetric(name, value, properties);
    }
}