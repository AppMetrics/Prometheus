// <copyright file="MetricsPrometheusPushGatewayReporter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;
using App.Metrics.Internal.NoOp;
using App.Metrics.Reporting;

namespace App.Metrics.Formatters.Prometheus
{
    public class MetricsPrometheusPushGatewayReporter : IReportMetrics
    {
        private static readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };

        public IFilterMetrics Filter { get; set; }

        public TimeSpan FlushInterval { get; set; }

        public IMetricsOutputFormatter Formatter { get; set; }

        private readonly Uri _targetUrl;

        public MetricsPrometheusPushGatewayReporter(MetricsPrometheusPushGatewayReporterSettings settings)
        {
            settings = settings ?? throw new ArgumentNullException(nameof(settings));

            if (string.IsNullOrEmpty(settings.Endpoint))
            {
                throw new ArgumentNullException(nameof(settings.Endpoint));
            }

            if (string.IsNullOrEmpty(settings.Job))
            {
                throw new ArgumentNullException(nameof(settings.Job));
            }

            var sb = new StringBuilder($"{settings.Endpoint.TrimEnd('/')}/metrics/job/{settings.Job}");

            if (!string.IsNullOrEmpty(settings.Instance))
            {
                sb.AppendFormat("/instance/{0}", settings.Instance);
            }

            if (settings.AdditionalLabels != null)
            {
                foreach (var label in settings.AdditionalLabels.Where(x => !string.IsNullOrWhiteSpace(x.Key) && !string.IsNullOrWhiteSpace(x.Value)))
                {
                    sb.AppendFormat("/{0}/{1}", label.Key, label.Value);
                }
            }

            if (!Uri.TryCreate(sb.ToString(), UriKind.Absolute, out _targetUrl))
            {
                throw new ArgumentException("Endpoint must be a valid url", nameof(settings.Endpoint));
            }

            Formatter = new MetricsPrometheusTextOutputFormatter();
            FlushInterval = TimeSpan.FromSeconds(20);
            Filter = new NullMetricsFilter();
        }

        public async Task<bool> FlushAsync(MetricsDataValueSource metricsData, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, _targetUrl);
            request.Content = BuildStreamContent(metricsData, cancellationToken);

            var response = await _httpClient.SendAsync(request, cancellationToken);

            return response.IsSuccessStatusCode;
        }

        private HttpContent BuildStreamContent(MetricsDataValueSource metrics, CancellationToken cancellationToken)
        {
            var memoryStream = new MemoryStream();
            Formatter.WriteAsync(memoryStream, metrics, cancellationToken);

            var content = new ByteArrayContent(memoryStream.ToArray());
            content.Headers.ContentType = new MediaTypeHeaderValue(Formatter.MediaType.ContentType);

            return content;
        }
    }
}