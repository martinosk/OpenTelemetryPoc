using Dafda.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using SomeMessages;

namespace AnEventConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddConsumer((f) =>
            {
                f.WithConfiguration("bootstrap.servers", "localhost");
                f.WithGroupId("aneventconsumergroup");
                f.RegisterMessageHandler<SomeMessage, SomeMessageHandler>("sometopic", SomeMessage.FriendlyName);
            });
            services.AddOpenTelemetry().WithTracing((builder) =>
            {
                builder
                .AddSource(nameof(SomeMessageHandler))
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("AnEventConsumer"))
                .AddJaegerExporter(opts =>
                {
                    opts.ExportProcessorType = ExportProcessorType.Simple;
                })
                .AddZipkinExporter();
            });
        });

    }
}
