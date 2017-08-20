// <copyright file="MetricsPrometheusMetricsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters.Prometheus;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up App Metrics Prometheus formatting services in an <see cref="IMetricsBuilder" />.
    /// </summary>
    public static class MetricsPrometheusMetricsBuilderExtensions
    {
        /// <summary>
        ///     Adds Prometheus formatters to the specified <see cref="IMetricsBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsBuilder" /> to add services to.</param>
        /// <returns>An <see cref="IMetricsBuilder"/> that can be used to further configure the App Metrics services.</returns>
        public static IMetricsBuilder AddPrometheusFormatters(this IMetricsBuilder builder)
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
        ///     Adds Prometheus options to the specified <see cref="IMetricsBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsBuilder" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action" /> to configure the provided <see cref="MetricsPrometheusOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsBuilder AddPrometheusOptions(
            this IMetricsBuilder builder,
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