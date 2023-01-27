public static class APIRegistrationExtensions
{
    public static void AddEndpoint<TEndpoint>(this WebApplication app)
        where TEndpoint : IGetEndpoint
    {
        app.MapGet(TEndpoint.Route, TEndpoint.Handler);
    }
}