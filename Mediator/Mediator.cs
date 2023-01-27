namespace Mediator;

internal class Mediator : IMediator
{
    readonly HandlersRegistry registry;

    IServiceProvider provider { get; init;}
    public Mediator(IServiceProvider provider, HandlersRegistry registry){
        this.provider = provider;
        this.registry = registry;
    }

    public TResponse RequestFor<TResponse>(IRequestFor<TResponse> request)
    {
        var registration = registry.RegistrationFor(request.GetType(), typeof(TResponse));
        var instance = provider.GetRequiredService(registration.consumerType);

        return (TResponse) registration.handler.Invoke(instance, new[]{ request })!;        
    }

}
