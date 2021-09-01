using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenTelemetry.Trace;
using OpenTelemetry;
using OpenTelemetry.Instrumentation.AspNetCore;
using System.Threading.Tasks;
using OpenTelemetry.Resources;

namespace MyApi
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
            //services.AddSingleton<ITracer>(services =>
            //{
            //    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            //    var reporter = new RemoteReporter.Builder().WithLoggerFactory(loggerFactory).WithSender(new UdpSender()).Build();
            //    var serviceName = services.GetRequiredService<IWebHostEnvironment>().ApplicationName;
            //    return new Tracer.Builder(serviceName)
            //    .WithSampler(new ConstSampler(true))
            //    .WithReporter(reporter)
            //    .Build();

            //});
            services.AddOpenTelemetryTracing(cfg => 
                cfg.AddAspNetCoreInstrumentation()
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MyApi"))
                .AddHttpClientInstrumentation()
                .AddJaegerExporter()
                .SetSampler(new AlwaysOnSampler()));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JaegerPoc", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JaegerPoc v1"));
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
