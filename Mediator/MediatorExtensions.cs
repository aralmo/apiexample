using Mediator;

public static class MediatorExtensions
{
    static HandlersRegistry registry = new();

    public static void AddMediator(this IServiceCollection services)
    {
        services.AddSingleton<IMediator, Mediator.Mediator>();
        services.AddSingleton(registry);
    }

    public static void AddMediatorObserver<TObserver>(this IServiceCollection services)
        where TObserver: class, IMediatorRequestObserver
        => services.AddSingleton<IMediatorRequestObserver, TObserver>();
    
    public static void AddHandler<TConsumer>(this IServiceCollection services)
        where TConsumer : IRequestHandler
    {
        var type = typeof(TConsumer);
        var implementedConsumers = type
            .GetInterfaces()
            .Where(IsGenericConsumerInterface);
        
        foreach(var implementedConsumer in implementedConsumers)
        {
            services.AddSingleton(typeof(TConsumer));

            registry.Register(                
                typeof(TConsumer),
                implementedConsumer.GenericTypeArguments[0],
                implementedConsumer.GenericTypeArguments[1]);
        }              

    }

    static bool IsGenericConsumerInterface(Type i)
    => typeof(IRequestHandler).IsAssignableFrom(i) && i.IsConstructedGenericType;
}