using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Instrumentation.Http;
using OpenTelemetry.Trace;

var serviceName = "WeatherAPI";
var serviceVersion = "1.0.0";

var builder = WebApplication.CreateBuilder(args);

//dependency injection
builder.Services.AddHandler<RandomWeatherForecastHandler>();
builder.Services.AddMediator();

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
    

var app = builder.Build();

//endpoints configuration
app.AddEndpoint<WeatherForecastEndpoint>();
app.AddEndpoint<WeatherForecastRangeEndpoint>();

app.Run();
