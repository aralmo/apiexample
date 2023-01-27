using Mediator;

public interface IWeatherForecastRequest:IRequestFor<IWeatherForecastResponse>
{
    string City{get;}
    DateOnly Date{get;}
}
