// <copyright file="PrometheusFormatterConstants.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Formatters.Prometheus.Internal
{
    public static class PrometheusFormatterConstants
    {
        public static readonly Func<string, string, string> MetricNameFormatter =
            (metricContext, metricName) => string.IsNullOrWhiteSpace(metricContext)
                ? $"{metricName}".Replace(' ', '_').ToLowerInvariant()
                : $"{metricContext}__{metricName}".Replace(' ', '_').ToLowerInvariant();
    }
}