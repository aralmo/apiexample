namespace Mediator;

internal class Mediator : IMediator
{
    readonly HandlersRegistry registry;
    readonly IMediatorRequestObserver[] requestObservers;
    IServiceProvider provider { get; init; }

    public Mediator(
        IServiceProvider provider,
        HandlersRegistry registry,
        IEnumerable<IMediatorRequestObserver> requestObservers)
    {
        this.provider = provider;
        this.registry = registry;
        this.requestObservers = requestObservers.ToArray();
    }

    public TResponse RequestFor<TResponse>(IRequestFor<TResponse> request)
    {
        var requestType = request.GetType();
        var registration = registry.RegistrationFor(requestType, typeof(TResponse));
        var instance = provider.GetRequiredService(registration.consumerType);

        RequestContext context = new()
        {
            Descriptor = new()
            {
                HandlerType = registration.consumerType,
                RequestType = requestType,
                ResponseType = typeof(TResponse)
            }
        };

        object?[] observerStates = requestObservers
            .Select(observer => observer.RequestStarted<TResponse>(context, request))
            .ToArray();

        TResponse? response = default;
        Exception? exception = null;
        try
        {
            response = (TResponse)registration.handler.Invoke(instance, new[] { request })!;
            return response;
        }
        catch (Exception ex)
        {
            exception = ex;
            throw;
        }
        finally
        {
            for (int n = 0; n < requestObservers.Count(); n++)
                requestObservers[n].RequestFinished<TResponse>(context, observerStates[n],response!, exception);
        }
    }

}
