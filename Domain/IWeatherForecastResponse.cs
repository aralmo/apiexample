public interface IWeatherForecastResponse
{
    string City{get;}
    DateOnly Date{get;}
    float TemperatureMinC {get;}
    float TemperatureMaxC {get;}
}