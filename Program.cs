var builder = WebApplication.CreateBuilder(args);

//dependency injection
builder.Services.AddHandler<RandomWeatherForecastHandler>();
builder.Services.AddMediator();

var app = builder.Build();

//endpoints configuration
app.AddEndpoint<WeatherForecastEndpoint>();
app.AddEndpoint<WeatherForecastRangeEndpoint>();

app.Run();
