namespace Mediator;

public interface IMediator
{
    Task<TResponse?> RequestFor<TResponse>(IRequestFor<TResponse> request);
}
public interface IRequestFor<TResponse>{ }
public interface IRequestHandler{
    Task<object?> Handle(object request);

}
public abstract class RequestHandler<TRequest, TResponse>:IRequestHandler
{
    async Task<object?> IRequestHandler.Handle(object request)
    => await Handle((TRequest) request);

    public abstract Task<TResponse?> Handle(TRequest request);
}
