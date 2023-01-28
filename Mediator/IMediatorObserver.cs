namespace Mediator;

public interface IMediatorRequestObserver
{
    object? RequestStarted<TResponse>(RequestContext context,  IRequestFor<TResponse> request);
    void RequestFinished<TResponse>(RequestContext context, object? observerState, TResponse response, Exception? exception);
}
public abstract class MediatorRequestObserver<TObserverState>:IMediatorRequestObserver
{
    public abstract TObserverState? RequestStarted<TResponse>(RequestContext context,  IRequestFor<TResponse> request);
    public abstract void RequestFinished<TResponse>(RequestContext context, TObserverState? state, TResponse response, Exception? exception);

    object? IMediatorRequestObserver.RequestStarted<TResponse>(RequestContext context, IRequestFor<TResponse> request)
    => RequestStarted<TResponse>(context, request);

    void IMediatorRequestObserver.RequestFinished<TResponse>(RequestContext context, object? observerState, TResponse response, Exception? exception)
    => RequestFinished<TResponse>(context,observerState != default? (TObserverState) observerState : default, response, exception);
}

public readonly struct RequestContext
{
    public readonly HandlerDescriptor Descriptor {get; init;}
}
public readonly struct HandlerDescriptor{
    public readonly Type HandlerType { get; init;}
    public readonly Type RequestType { get; init;}
    public readonly Type ResponseType { get; init;}
}