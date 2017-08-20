// <copyright file="MetricsPrometheusOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.Options;

namespace App.Metrics.Formatters.Prometheus.Internal
{
    /// <summary>
    ///     Sets up default Prometheus options for <see cref="MetricsOptions"/>.
    /// </summary>
    public class MetricsPrometheusOptionsSetup : IConfigureOptions<MetricsOptions>
    {
        private readonly MetricsPrometheusOptions _prometheusOptions;

        public MetricsPrometheusOptionsSetup(IOptions<MetricsPrometheusOptions> prometheusOptionsAccessor)
        {
            _prometheusOptions = prometheusOptionsAccessor.Value ?? throw new ArgumentNullException(nameof(prometheusOptionsAccessor));
        }

        /// <inheritdoc/>
        public void Configure(MetricsOptions options)
        {
            var protobufFormatter = new MetricsPrometheusProtobufOutputFormatter(_prometheusOptions);
            var textFormatter = new MetricsPrometheusTextOutputFormatter(_prometheusOptions);

            options.OutputMetricsFormatters.Add(protobufFormatter);
            options.OutputMetricsFormatters.Add(textFormatter);
        }
    }
}