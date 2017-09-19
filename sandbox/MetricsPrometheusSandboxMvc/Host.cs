// <copyright file="Host.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;

namespace MetricsPrometheusSandboxMvc
{
    public static class Host
    {
        public static IWebHost BuildWebHost(string[] args)
        {
            ConfigureLogging();

            return WebHost.CreateDefaultBuilder(args)
                          .ConfigureMetricsWithDefaults(
                              builder =>
                              {
                                  builder.OutputMetrics.AsPrometheusPlainText();
                                  builder.OutputMetrics.AsPrometheusProtobuf();
                              })
                          .UseMetrics(
                              options =>
                              {
                                  options.EndpointOptions = endpointsOptions =>
                                  {
                                      // TODO: provide an easier way?
                                      endpointsOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
                                      endpointsOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
                                  };
                              })
                          .UseSerilog()
                          .UseStartup<Startup>()
                          .Build();
        }

        public static void Main(string[] args) { BuildWebHost(args).Run(); }

        private static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
                .WriteTo.LiterateConsole()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();
        }
    }
}