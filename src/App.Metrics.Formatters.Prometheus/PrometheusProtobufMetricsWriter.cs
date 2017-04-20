// <copyright file="PrometheusProtobufMetricsWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Extensions.Middleware.Abstractions;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.Formatters.Prometheus
{
    // ReSharper disable UnusedMember.Global
    public class PrometheusProtobufMetricsWriter : IMetricsResponseWriter
        // ReSharper restore UnusedMember.Global
    {
        public Task WriteAsync(HttpContext context, MetricsDataValueSource metricsData, CancellationToken token = default(CancellationToken))
        {
            var bodyData = ProtoFormatter.Format(metricsData.GetPrometheusMetricsSnapshot());
            return context.Response.Body.WriteAsync(bodyData, 0, bodyData.Length, token);
        }

        public string ContentType => "application/vnd.google.protobuf; proto=io.prometheus.client.MetricFamily; encoding=delimited";
    }
}