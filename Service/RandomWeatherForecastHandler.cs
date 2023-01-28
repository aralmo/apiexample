using Mediator;

public class RandomWeatherForecastHandler :
    RequestHandler<IWeatherForecastRequest, IWeatherForecastResponse>
{
    //this could be a call to an api or we
    public override Task<IWeatherForecastResponse?> Handle(IWeatherForecastRequest request)
     =>Task.FromResult((IWeatherForecastResponse?) 
        new ForecastResponse(
            City: request.City,
            Date: request.Date,
            TemperatureMaxC: Random.Shared.Next(-20, 55),
            TemperatureMinC: Random.Shared.Next(-20, 55)));
    
    private record ForecastResponse(string City, DateOnly Date, float TemperatureMinC,float TemperatureMaxC) : IWeatherForecastResponse;
}