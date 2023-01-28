
using System.Diagnostics;
using Mediator;

public class OpenWeatherForecastHandler
: RequestHandler<IWeatherForecastRequest, IWeatherForecastResponse>
{
    private readonly IHttpClientFactory httpFactory;

    public OpenWeatherForecastHandler(IHttpClientFactory httpFactory)
    {
        this.httpFactory = httpFactory;
    }

    public override async Task<IWeatherForecastResponse?> Handle(IWeatherForecastRequest request)
    {
        using var client = httpFactory.CreateClient();

        var cityData =
            (await SearchCity(client, request.City))
            ?? throw new BadRequestException($"city {request.City} not found", nameof(request.City));

        return await GetForecast(client, cityData.latitude, cityData.longitude, cityData.timezone, cityData.name, request.Date);
    }

    static async Task<IWeatherForecastResponse> GetForecast(HttpClient client, double latitude, double longitude, string timezone, string cityName, DateOnly date)
    {
        var forecastUri = HttpUriUtility.BuildUri(
            "https://api.open-meteo.com/v1/forecast",
            ("latitude", latitude),
            ("longitude", longitude),
            ("current_weather", "false"),
            ("timezone", timezone),
            ("start_date", date.ToString("yyyy-MM-dd")),
            ("end_date", date.ToString("yyyy-MM-dd")));
        var forecastResponse = await client.GetFromJsonAsync<OpenMeteoResponse>(
            $"{forecastUri.ToString()}&daily=temperature_2m_max,temperature_2m_min");
        
        return new ForecastResponse(
            cityName, 
            date, 
            forecastResponse.Daily.temperature_2m_min.First(),  
            forecastResponse.Daily.temperature_2m_max.First());
    }
    record ForecastResponse(string City, DateOnly Date,float TemperatureMinC, float TemperatureMaxC) : IWeatherForecastResponse;
   
    struct OpenMeteoResponse
    {
        public Daily Daily { get; set; }
    }
    struct Daily
    {
        public float[] temperature_2m_max { get; set; }
        public float[] temperature_2m_min { get; set; }
    }

    static async Task<(string name, double latitude, double longitude, string timezone)?> SearchCity(HttpClient client, string city)
    {
        var geocodeUri = HttpUriUtility.BuildUri(
            "https://geocoding-api.open-meteo.com/v1/search",
            ("name", city));

        return (await client.GetFromJsonAsync<GeoCodingResponse>(geocodeUri))
            .Results?
            .Select(cityInfo => (
                name: cityInfo.Name,
                latitude: cityInfo.Latitude,
                longitude: cityInfo.Longitude,
                timezone: cityInfo.Timezone))
            .FirstOrDefault() ?? null;
    }

    struct GeoCodingResponse
    {
        public IEnumerable<GeoCodingResult> Results { get; set; }
    }
    struct GeoCodingResult
    {
        public string Name { get; set; } //city name
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Timezone { get; set; }
    }

}