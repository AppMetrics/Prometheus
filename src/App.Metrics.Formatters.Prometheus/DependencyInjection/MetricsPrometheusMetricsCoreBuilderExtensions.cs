// <copyright file="MetricsPrometheusMetricsCoreBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters.Prometheus;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up App Metrics Prometheus formatting services in an <see cref="IMetricsCoreBuilder" />.
    /// </summary>
    public static class MetricsPrometheusMetricsCoreBuilderExtensions
    {
        /// <summary>
        ///     Adds Prometheus formatters to the specified <see cref="IMetricsCoreBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsCoreBuilder" /> to add services to.</param>
        /// <returns>An <see cref="IMetricsCoreBuilder"/> that can be used to further configure App Metrics services.</returns>
        public static IMetricsCoreBuilder AddPrometheusFormattersCore(this IMetricsCoreBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddPrometheusFormatterServices();
            builder.Services.AddDefaultFormatterOptions();

            return builder;
        }

        /// <summary>
        ///     Adds Prometheus options to the specified <see cref="IMetricsCoreBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsCoreBuilder" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action" /> to configure the provided <see cref="MetricsPrometheusOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsCoreBuilder" /> that can be used to further configure App Metrics services.
        /// </returns>
        public static IMetricsCoreBuilder AddPrometheusOptionsCore(
            this IMetricsCoreBuilder builder,
            Action<MetricsPrometheusOptions> setupAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Configure(setupAction);

            return builder;
        }
    }
}
