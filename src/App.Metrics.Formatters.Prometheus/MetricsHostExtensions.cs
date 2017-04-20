// <copyright file="MetricsHostExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Extensions.Middleware.Abstractions;
using App.Metrics.Formatters.Prometheus;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    // ReSharper restore CheckNamespace

    public static class MetricsHostExtensions
    {
        public static IMetricsHostBuilder AddPrometheusPlainTextSerialization(this IMetricsHostBuilder host)
        {
            host.Services.Replace(ServiceDescriptor.Transient<IMetricsResponseWriter, PrometheusPlainTextMetricsWriter>());

            return host;
        }

        public static IMetricsHostBuilder AddPrometheusProtobufSerialization(this IMetricsHostBuilder host)
        {
            host.Services.Replace(ServiceDescriptor.Transient<IMetricsResponseWriter, PrometheusProtobufMetricsWriter>());

            return host;
        }
    }
}