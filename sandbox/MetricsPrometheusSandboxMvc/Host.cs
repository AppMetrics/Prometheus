// <copyright file="Host.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Endpoints;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace MetricsPrometheusSandboxMvc
{
    public static class Host
    {
        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost
                .CreateDefaultBuilder(args)
                .ConfigureServices(AddMetricsOptions)
                .UseMetrics(ConfigureMetricsOptions())
                .UseStartup<Startup>()
                .Build();
        }

        public static void Main(string[] args) { BuildWebHost(args).Run(); }

        private static void AddMetricsOptions(IServiceCollection services)
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MetricsEndpointsOptions>, MetricsEndpointsOptionsSetup>());
        }

        private static Action<MetricsWebHostOptions> ConfigureMetricsOptions()
        {
            return options =>
            {
                options.CoreBuilder.AddPrometheusFormattersCore();
            };
        }
    }
}