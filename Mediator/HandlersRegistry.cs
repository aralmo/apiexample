using System.Reflection;

namespace Mediator;

internal class HandlersRegistry
{
    List<(Type request, Type response,Type consumer, MethodInfo handler)> consumerTypes = new();

    internal void Register(Type consumerType, Type requestType, Type responseType)
    => consumerTypes.Add((
        request: requestType, 
        response: responseType, 
        consumer: consumerType, 
        handler: HandlerInvocation(consumerType)));

    private MethodInfo HandlerInvocation(Type consumerType)
        => consumerType.GetMethod("Handle", BindingFlags.Instance | BindingFlags.Public)!;

    internal (Type request, Type response,Type consumer, MethodInfo handler) RegistrationFor(Type request, Type response)
        => consumerTypes
            .Where(ct => 
                ct.request.IsAssignableFrom(request) &&
                ct.response.IsAssignableFrom(response))
            .FirstOrDefault();
            
                   
                
                
}