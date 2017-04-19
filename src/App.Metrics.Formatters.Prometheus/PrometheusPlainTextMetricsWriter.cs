// <copyright file="PrometheusPlainTextMetricsWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Extensions.Middleware.Abstractions;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.Extensions.Reporting.Prometheus
{
    public class PrometheusPlainTextMetricsWriter : IMetricsResponseWriter
    {
        public Task WriteAsync(HttpContext context, MetricsDataValueSource metricsData, CancellationToken token = default(CancellationToken))
        {
            return context.Response.WriteAsync(AsciiFormatter.Format(metricsData.GetPrometheusMetricsSnapshot()), token);
        }

        public string ContentType => "text/plain";
    }
}