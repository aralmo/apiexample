using System.Diagnostics;

namespace Mediator.Telemetry;

public class MediatorTelemetryObserver : MediatorRequestObserver<Activity>
{
    private readonly ActivitySource source;

    public MediatorTelemetryObserver(ActivitySource source)
    {
        this.source = source;
    }

    public override Activity? RequestStarted<TResponse>(RequestContext context, IRequestFor<TResponse> request)
    {
         var activity = source.StartActivity($"mediator {context.Descriptor.RequestType.Name}");
         activity?.AddEvent(new ActivityEvent($"Handled by {context.Descriptor.HandlerType}"));
         return activity;
    }

    public override void RequestFinished<TResponse>(RequestContext context, Activity? activity, TResponse response, Exception? exception)
    {
        if (activity != null)
        {
            if (exception == null)
            {
                activity.SetTag("otel.status_code", "OK");
            }
            else
            {
                activity.AddEvent(new ActivityEvent("Threw an exception"));
                activity.SetTag("otel.status_code", "ERROR");
                activity.SetTag("otel.status_description", exception.Message);
            }

            activity.Stop();

        }

    }
}
