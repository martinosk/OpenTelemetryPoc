using Dafda.Producing;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using SomeMessages;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SomeOtherApi
{
    public class MessageProducer
    {
        private readonly Producer producer;
        private static readonly ActivitySource activity = new(nameof(MessageProducer));
        private static readonly TextMapPropagator propagator = Propagators.DefaultTextMapPropagator;

        public MessageProducer(Producer producer)
        {
            this.producer = producer;
        }

        public async Task Produce(SomeMessage message)
        {
            var metadata = new Dictionary<string, object>();
            using var currentActivity = activity.StartActivity("Kafka Produce", ActivityKind.Producer);
            propagator.Inject(new PropagationContext(currentActivity.Context, Baggage.Current), message.TraceMetadata, InjectTraceContextIntoMessage);
            currentActivity.SetTag("messaging.system", "kafka");
            currentActivity.SetTag("messaging.destination_kind", "topic");
            currentActivity.SetTag("messaging.kafka.topic", "sometopic");
            await producer.Produce(message, metadata);
        }

        private void InjectTraceContextIntoMessage(Dictionary<string, object> metadata, string key, string value)
        {
            metadata[key] = value;
        }
    }
}