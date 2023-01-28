using Mediator;

public class WeatherForecastEndpoint : IGetEndpoint
{
    public static string Route => "/forecast/{city}/{date}";

    public static Delegate Handler
    => (IMediator mediator,
        DateParameter date,
        string city)
    => mediator.RequestFor(new ForecastRequest(city, date));

    private record ForecastRequest(string City, DateOnly Date) : IWeatherForecastRequest;
}
