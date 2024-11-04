using Core.OpenTelemetry;
using Marten;
using System.Diagnostics;

namespace Core.Marten.OpenTelemetry
{
    internal static class MartenActivityScope
    {
        internal const string ActivitySourceName = "Marten.Diagnostics";

        private static ActivitySource ActivitySource { get; } = new(ActivitySourceName);
        internal static Activity? StartActivity(string name)
        {
            Activity? activity = ActivitySource.StartActivity($"MartenRepository/{name}");
            if (activity == null) return null;

            return activity;
        }
        
        internal static void PropagateTelemetry(this IDocumentSession documentSession, Activity? activity)
        {
            var propagationContext = activity.Propagate(
                    documentSession,
                    (session, key, value) => session.SetHeader(key, value)
                );

            if (!propagationContext.HasValue) return;

            documentSession.CorrelationId = propagationContext.Value.ActivityContext.TraceId.ToHexString();
            documentSession.CausationId = propagationContext.Value.ActivityContext.SpanId.ToHexString();
        }
    }
}
