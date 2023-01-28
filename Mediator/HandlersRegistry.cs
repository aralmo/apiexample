using System.Reflection;

namespace Mediator;

internal class HandlersRegistry
{
    List<(Type request, Type response,Type consumer)> consumerTypes = new();

    internal void Register(Type consumerType, Type requestType, Type responseType)
    => consumerTypes.Add((
        request: requestType, 
        response: responseType, 
        consumer: consumerType));

    internal (Type request, Type response,Type consumer) RegistrationFor(Type request, Type response)
        => consumerTypes
            .Where(ct => 
                ct.request.IsAssignableFrom(request) &&
                ct.response.IsAssignableFrom(response))
            .FirstOrDefault();
            
                   
                
                
}