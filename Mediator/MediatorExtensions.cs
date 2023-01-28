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
        where TObserver : class, IMediatorRequestObserver
        => services.AddSingleton<IMediatorRequestObserver, TObserver>();

    public static void AddHandler<TConsumer>(this IServiceCollection services)
        where TConsumer : IRequestHandler
    {
        var type = typeof(TConsumer).BaseType;
        if (type?.GetGenericTypeDefinition() != typeof(RequestHandler<,>))
            throw new InvalidOperationException($"{typeof(TConsumer).Name} must inherit from abstract RequestHandler<,>");

        services.AddSingleton(typeof(TConsumer));

        registry.Register(
            typeof(TConsumer),
            type.GenericTypeArguments[0],
            type.GenericTypeArguments[1]);
    }

    static bool IsConsumerInterface(Type i)
    => typeof(IRequestHandler).IsAssignableFrom(i);
}