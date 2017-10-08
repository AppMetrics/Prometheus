// <copyright file="AppMetricsMiddlewarePrometheusOptionsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Builder;
using App.Metrics.Formatters.Prometheus;
using App.Metrics.Middleware;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public static class AppMetricsMiddlewarePrometheusOptionsBuilderExtensions
    {
        /// <summary>
        /// Enables InfluxDB's Line Protocol serialization on the metric endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options checksBuilder.</param>
        /// <returns>The metrics host builder</returns>
        public static IAppMetricsMiddlewareOptionsBuilder AddMetricsPrometheusFormatters(this IAppMetricsMiddlewareOptionsBuilder options)
        {
            options.AppMetricsBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricsResponseWriter, PrometheusProtobufMetricsWriter>());

            return options;
        }

        /// <summary>
        /// Enables InfluxDB's Line Protocol serialization on the metric endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options checksBuilder.</param>
        /// <returns>The metrics host builder</returns>
        public static IAppMetricsMiddlewareOptionsBuilder AddMetricsTextPrometheusFormatters(this IAppMetricsMiddlewareOptionsBuilder options)
        {
            options.AppMetricsBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricsTextResponseWriter, PrometheusPlainTextMetricsWriter>());

            return options;
        }

        /// <summary>
        /// Enables InfluxDB's Line Protocol serialization on the metrics and metrics-text responses
        /// </summary>
        /// <param name="options">The metrics middleware options checksBuilder.</param>
        /// <returns>The metrics host builder</returns>
        public static IAppMetricsMiddlewareOptionsBuilder AddPrometheusFormatters(this IAppMetricsMiddlewareOptionsBuilder options)
        {
            options.AddMetricsPrometheusFormatters();
            options.AddMetricsTextPrometheusFormatters();

            return options;
        }
    }
}