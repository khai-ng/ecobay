using OpenTelemetry.Trace;

namespace Core.Kafka.OpenTelemetry
{
    public static class ConfluentKafkaInstrumentation
    {
        public static TracerProviderBuilder AddConfluentKafkaInstrumentation(this TracerProviderBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddSource(ActivitySourceAccessor.ActivitySourceName);

            return builder;
        }
    }
}
