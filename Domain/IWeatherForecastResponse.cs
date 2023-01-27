public interface IWeatherForecastResponse
{
    string City{get;}
    DateOnly Date{get;}
    int TemperatureC {get;}
}