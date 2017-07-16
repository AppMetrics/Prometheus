// <copyright file="Startup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using App.Metrics.Builder;
using App.Metrics.Sandbox.JustForTesting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Sandbox
{
    public class Startup
    {
        private static readonly bool HaveAppRunSampleRequests = true;
        private static readonly bool RunSamplesWithClientId = true;

        public Startup(IConfiguration configuration) { Configuration = configuration; }

        public IConfiguration Configuration { get; }

        public static IWebHost BuildSandboxWebHost(string[] args)
        {
            return new WebHostBuilder().UseContentRoot(Directory.GetCurrentDirectory()).
                                        ConfigureAppConfiguration(
                                            (context, builder) =>
                                            {
                                                builder.SetBasePath(context.HostingEnvironment.ContentRootPath).
                                                        AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).
                                                        AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true).
                                                        AddEnvironmentVariables();
                                            }).
                                        ConfigureLogging(
                                            factory =>
                                            {
                                                factory.AddConsole();
                                            }).
                                        UseIISIntegration().
                                        UseKestrel().
                                        UseStartup<Startup>().
                                        Build();
        }

        public static void Main(string[] args) { BuildSandboxWebHost(args).Run(); }

        public void Configure(IApplicationBuilder app, IApplicationLifetime lifetime)
        {
            if (RunSamplesWithClientId && HaveAppRunSampleRequests)
            {
                app.Use(
                    (context, func) =>
                    {
                        RandomClientIdForTesting.SetTheFakeClaimsPrincipal(context);
                        return func();
                    });
            }

            app.UseMetrics();

            app.UseMvc();

            if (HaveAppRunSampleRequests)
            {
                SampleRequests.Run(lifetime.ApplicationStopping);
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTestStuff();
            services.AddLogging().AddRouting(options => { options.LowercaseUrls = true; });

            services.AddMvc(options => options.AddMetricsResourceFilter());

            services.AddMetrics(Configuration.GetSection("AppMetrics")).
                         AddMetricsMiddleware(
                         Configuration.GetSection("AspNetMetrics"),
                         optionsBuilder =>
                         {
                             optionsBuilder.AddMetricsPrometheusFormatters().
                                            AddMetricsTextPrometheusFormatters().
                                            AddEnvironmentAsciiFormatters();
                         });

            services.
                AddHealthChecks().
                AddHealthCheckMiddleware(optionsBuilder => optionsBuilder.AddAsciiFormatters()).
                AddChecks(registry =>
                {
                    registry.AddPingCheck("google ping", "google.com", TimeSpan.FromSeconds(10));
                    registry.AddHttpGetCheck("github", new Uri("https://github.com/"), TimeSpan.FromSeconds(10));
                });
        }
    }
}