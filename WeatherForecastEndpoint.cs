using Mediator;
using Microsoft.AspNetCore.Mvc;

public class WeatherForecastEndpoint : IGetEndpoint
{
    public static string Route => "/forecast";

    public static Delegate Handler
    => (IMediator mediator, 
        [FromQuery(Name = "d")] DateParameter date, 
        [FromQuery(Name = "c")] string city)
    => mediator.RequestFor(new ForecastRequest(city, date));

    private record ForecastRequest(string City, DateOnly Date) : IWeatherForecastRequest;
}
