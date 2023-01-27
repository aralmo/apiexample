namespace Mediator;

public interface IMediator
{
    TResponse RequestFor<TResponse>(IRequestFor<TResponse> request);
}
public interface IRequestFor<TResponse>{ }
public interface IRequestHandler{}
public interface IRequestHandler<TRequest, TResponse>:IRequestHandler
{
    abstract TResponse Handle(TRequest request);
}
