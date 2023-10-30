using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Dafda.Configuration;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using SomeMessages;

namespace SomeOtherApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddProducerFor<MessageProducer>(options =>
            {
                options.WithConfiguration("bootstrap.servers", "localhost");
                options.Register<SomeMessage>("sometopic", "some_message", x => x.Id.ToString());
            });
            services.AddOpenTelemetry().WithTracing(cfg =>
                cfg.AddAspNetCoreInstrumentation()
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("SomeOtherApi"))
                .AddHttpClientInstrumentation()
                .AddSource(nameof(MessageProducer))
                .AddJaegerExporter()
                .AddZipkinExporter()
                .AddConsoleExporter()
                .SetSampler(new AlwaysOnSampler()));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SomeOtherApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SomeOtherApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
