using Mediator;
using Microsoft.AspNetCore.Mvc;

public class WeatherForecastRangeEndpoint : IGetEndpoint
{
    public static string Route => "/forecastrange";
    public static Delegate Handler
    => (IMediator mediator,
        [FromQuery(Name = "from")] DateParameter dateFrom,
        [FromQuery(Name = "to")] DateParameter dateTo,
        [FromQuery(Name = "c")] string city)
        => new
        {
            forecasts = Enumerable
            .Range(dateFrom.Date.DayNumber, dateTo.Date.DayNumber - dateFrom.Date.DayNumber)
            .Select(day => mediator
            .RequestFor(
                new ForecastRequest(
                    City: city,
                    Date: DateOnly.FromDayNumber(day))))
        };

    private record ForecastRequest(string City, DateOnly Date) : IWeatherForecastRequest;
}