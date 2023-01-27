using Mediator;

public class RandomWeatherForecastHandler :
    IRequestHandler<IWeatherForecastRequest, IWeatherForecastResponse>
{
    //this could be a call to an api or we
    public IWeatherForecastResponse Handle(IWeatherForecastRequest request)
     => new ForecastResponse(
        City: request.City,
        Date: request.Date,
        TemperatureC: Random.Shared.Next(-20, 55));
    
    private record ForecastResponse(string City, DateOnly Date, int TemperatureC) : IWeatherForecastResponse;
}