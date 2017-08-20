// <copyright file="MetricsPrometheusProtobufOutputFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Formatters.Prometheus.Internal;
using App.Metrics.Formatters.Prometheus.Internal.Extensions;

namespace App.Metrics.Formatters.Prometheus
{
    public class MetricsPrometheusProtobufOutputFormatter : IMetricsOutputFormatter
    {
        private readonly MetricsPrometheusOptions _options;

        public MetricsPrometheusProtobufOutputFormatter()
        {
            _options = new MetricsPrometheusOptions();
        }

        public MetricsPrometheusProtobufOutputFormatter(MetricsPrometheusOptions options) { _options = options ?? throw new ArgumentNullException(nameof(options)); }

        /// <inheritdoc/>
        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("text", "vnd.appmetrics.metrics.prometheus", "v1", "plain");

        /// <inheritdoc/>
        public Task WriteAsync(
            Stream output,
            MetricsDataValueSource metricsData,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            using (var writer = new BinaryWriter(output))
            {
                writer.Write(ProtoFormatter.Format(metricsData.GetPrometheusMetricsSnapshot(_options.MetricNameFormatter)));
            }

            return Task.CompletedTask;
        }
    }
}