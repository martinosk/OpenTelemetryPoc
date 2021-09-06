using Dafda.Consuming;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using SomeMessages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnEventConsumer
{
    public class SomeMessageHandler : IMessageHandler<SomeMessage>
    {
        private static readonly ActivitySource activity = new(nameof(SomeMessageHandler));
        private static readonly TextMapPropagator propagator = new TraceContextPropagator();

        public Task Handle(SomeMessage message, MessageHandlerContext context)
        {
            var parentContext = propagator.Extract(default,
                message,
                ExtractTraceContextFromBasicProperties);
            Baggage.Current = parentContext.Baggage;
            using var currentActivity = activity.StartActivity("Handle message", ActivityKind.Consumer, parentContext.ActivityContext);
            currentActivity.SetTag("messaging.system", "kafka");
            currentActivity.SetTag("messaging.destination_kind", "topic");
            currentActivity.SetTag("messaging.kafka.topic", "sometopic");

            Console.WriteLine($"Handled message {message.Id}");
            return Task.CompletedTask;
        }

        private IEnumerable<string> ExtractTraceContextFromBasicProperties(SomeMessage message, string key)
        {
            if (message.TraceMetadata.TryGetValue(key, out var value))
            {
                yield return value.ToString();
            }
        }
    }
}
