using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Instrumentation.Http;
using OpenTelemetry.Trace;
using Mediator.Telemetry;

var serviceName = "WeatherAPI";
var serviceVersion = "1.0.0";

var builder = WebApplication.CreateBuilder(args);

//dependency injection


//opentelemetry
builder.Services
    .AddOpenTelemetry()
    .WithTracing(options =>
    {
        options
            .AddSource(serviceName)
            .AddConsoleExporter()
            .SetResourceBuilder(ResourceBuilder
                    .CreateDefault()
                    .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation(options => { });
    })
    .StartWithHost();    


//mediator
builder.Services.AddHandler<RandomWeatherForecastHandler>();
builder.Services.AddMediator();
builder.Services.AddMediatorObserver<MediatorTelemetryObserver>();

var app = builder.Build();

//endpoints configuration
app.AddEndpoint<WeatherForecastEndpoint>();
app.AddEndpoint<WeatherForecastRangeEndpoint>();

app.Run();
