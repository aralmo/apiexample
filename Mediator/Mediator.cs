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

    public async Task<TResponse?> RequestFor<TResponse>(IRequestFor<TResponse> request)
    {
        var requestType = request.GetType();
        var registration = registry.RegistrationFor(requestType, typeof(TResponse));
        var instance = provider.GetRequiredService(registration.consumer) as IRequestHandler;

        RequestContext context = new()
        {
            Descriptor = new()
            {
                HandlerType = registration.consumer,
                RequestType = registration.request,
                ResponseType =registration.response
            }
        };

        object?[] observerStates = requestObservers
            .Select(observer => observer.RequestStarted<TResponse>(context, request))
            .ToArray();

        TResponse? response = default;
        Exception? exception = null;
        try
        {
            response = (TResponse?) await instance!.Handle(request);
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
