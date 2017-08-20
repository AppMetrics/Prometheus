// <copyright file="MetricsPrometheusFormattingServiceCollectionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics;
using App.Metrics.Formatters.Prometheus;
using App.Metrics.Formatters.Prometheus.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up App Metrics Prometheus formatting services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class MetricsPrometheusFormattingServiceCollectionExtensions
    {
        internal static void AddPrometheusFormatterServices(this IServiceCollection services)
        {
            var prometheusSetupOptionsDescriptor =
                ServiceDescriptor.Transient<IConfigureOptions<MetricsOptions>, MetricsPrometheusOptionsSetup>();
            services.TryAddEnumerable(prometheusSetupOptionsDescriptor);
        }

        internal static void AddDefaultFormatterOptions(this IServiceCollection services)
        {
            services.Configure<MetricsOptions>(
                options =>
                {
                    if (options.DefaultOutputMetricsFormatter == null)
                    {
                        options.DefaultOutputMetricsFormatter = options.OutputMetricsFormatters.GetType<MetricsPrometheusProtobufOutputFormatter>();
                    }
                });
        }
    }
}