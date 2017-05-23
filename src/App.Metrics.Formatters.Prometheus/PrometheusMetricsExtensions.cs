// <copyright file="PrometheusMetricsExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using App.Metrics.Apdex;
using App.Metrics.Core;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Tagging;
using App.Metrics.Timer;

namespace App.Metrics.Formatters.Prometheus
{
    public static class PrometheusMetricsExtensions
    {
        private static readonly Regex Rgx = new Regex("[^a-z0-9A-Z:_]");

        public static IEnumerable<MetricFamily> GetPrometheusMetricsSnapshot(this MetricsDataValueSource snapshot)
        {
            var result = new List<MetricFamily>();
            foreach (var group in snapshot.Contexts)
            {
                foreach (var metricGroup in group.ApdexScores.GroupBy(
                    source => source.IsMultidimensional ? source.MultidimensionalName : source.Name))
                {
                    var promMetricFamily = new MetricFamily
                                           {
                                               name = FormatName($"{group.Context}_{metricGroup.Key}"),
                                               type = MetricType.GAUGE
                                           };
                    foreach (var metric in metricGroup)
                    {
                        promMetricFamily.metric.AddRange(metric.ToPrometheusMetrics());
                    }

                    result.Add(promMetricFamily);
                }

                foreach (var metricGroup in group.Gauges.GroupBy(
                    source => source.IsMultidimensional ? source.MultidimensionalName : source.Name))
                {
                    var promMetricFamily = new MetricFamily
                                           {
                                               name = FormatName($"{group.Context}_{metricGroup.Key}"),
                                               type = MetricType.GAUGE
                                           };
                    foreach (var metric in metricGroup)
                    {
                        promMetricFamily.metric.AddRange(metric.ToPrometheusMetrics());
                    }

                    result.Add(promMetricFamily);
                }

                foreach (var metricGroup in group.Counters.GroupBy(
                    source => source.IsMultidimensional ? source.MultidimensionalName : source.Name))
                {
                    var promMetricFamily = new MetricFamily
                                           {
                                               name = FormatName($"{group.Context}_{metricGroup.Key}"),
                                               type = MetricType.GAUGE
                                           };

                    foreach (var metric in metricGroup)
                    {
                        promMetricFamily.metric.AddRange(metric.ToPrometheusMetrics());
                    }

                    result.Add(promMetricFamily);
                }

                foreach (var metricGroup in group.Meters.GroupBy(
                    source => source.IsMultidimensional ? source.MultidimensionalName : source.Name))
                {
                    var promMetricFamily = new MetricFamily
                                           {
                                               name = FormatName($"{group.Context}_{metricGroup.Key}_total"),
                                               type = MetricType.COUNTER
                                           };

                    foreach (var metric in metricGroup)
                    {
                        promMetricFamily.metric.AddRange(metric.ToPrometheusMetrics());
                    }

                    result.Add(promMetricFamily);
                }

                foreach (var metricGroup in group.Histograms.GroupBy(
                    source => source.IsMultidimensional ? source.MultidimensionalName : source.Name))
                {
                    var promMetricFamily = new MetricFamily
                                           {
                                               name = FormatName($"{group.Context}_{metricGroup.Key}"),
                                               type = MetricType.SUMMARY,
                                           };

                    foreach (var timer in metricGroup)
                    {
                        promMetricFamily.metric.AddRange(timer.ToPrometheusMetrics());
                    }

                    result.Add(promMetricFamily);
                }

                foreach (var metricGroup in group.Timers.GroupBy(
                    source => source.IsMultidimensional ? source.MultidimensionalName : source.Name))
                {
                    var promMetricFamily = new MetricFamily
                                           {
                                               name = FormatName($"{group.Context}_{metricGroup.Key}"),
                                               type = MetricType.SUMMARY,
                                           };

                    foreach (var timer in metricGroup)
                    {
                        promMetricFamily.metric.AddRange(timer.ToPrometheusMetrics());
                    }

                    result.Add(promMetricFamily);
                }
            }

            return result;
        }

        public static List<LabelPair> ToLabelPairs(this MetricTags tags)
        {
            var result = new List<LabelPair>(tags.Count);
            for (var i = 0; i < tags.Count; i++)
            {
                result.Add(new LabelPair() { name = tags.Keys[i], value = tags.Values[i] });
            }

            return result;
        }

        public static Metric ToPrometheusMetric(this CounterValue.SetItem item)
        {
            var result = new Metric()
                         {
                             gauge = new Gauge()
                                     {
                                         value = item.Count
                                     },
                             label = item.Tags.ToLabelPairs()
                         };

            return result;
        }

        public static Metric ToPrometheusMetric(this MeterValue.SetItem item)
        {
            var result = new Metric()
                         {
                             counter = new Counter()
                                       {
                                           value = item.Value.Count
                                       },
                             label = item.Tags.ToLabelPairs()
                         };

            return result;
        }

        public static List<Metric> ToPrometheusMetrics(this ApdexValueSource metric)
        {
            var result = new List<Metric>
                         {
                             new Metric()
                             {
                                 gauge = new Gauge()
                                         {
                                             value = metric.Value.Score
                                         },
                                 label = metric.Tags.ToLabelPairs()
                             }
                         };

            return result;
        }

        public static List<Metric> ToPrometheusMetrics(this GaugeValueSource metric)
        {
            var result = new List<Metric>
                         {
                             new Metric()
                             {
                                 gauge = new Gauge()
                                         {
                                             value = metric.Value
                                         },
                                 label = metric.Tags.ToLabelPairs()
                             }
                         };

            return result;
        }

        public static List<Metric> ToPrometheusMetrics(this CounterValueSource metric)
        {
            var result = new List<Metric>
                         {
                             new Metric()
                             {
                                 gauge = new Gauge()
                                         {
                                             value = metric.Value.Count
                                         },
                                 label = metric.Tags.ToLabelPairs()
                             }
                         };

            if (metric.Value.Items?.Length > 0)
            {
                result.AddRange(metric.Value.Items.Select(ToPrometheusMetric));
            }

            return result;
        }

        public static List<Metric> ToPrometheusMetrics(this MeterValueSource metric)
        {
            var result = new List<Metric>
                         {
                             new Metric()
                             {
                                 counter = new Counter()
                                           {
                                               value = metric.Value.Count
                                           },
                                 label = metric.Tags.ToLabelPairs()
                             }
                         };

            if (metric.Value.Items?.Length > 0)
            {
                result.AddRange(metric.Value.Items.Select(x => x.ToPrometheusMetric()));
            }

            return result;
        }

        public static List<Metric> ToPrometheusMetrics(this HistogramValueSource metric)
        {
            var result = new List<Metric>
                         {
                             new Metric()
                             {
                                 summary = new Summary()
                                           {
                                               sample_count = (ulong)metric.Value.Count,
                                               sample_sum = metric.Value.Sum,
                                               quantile =
                                               {
                                                   new Quantile() { quantile = 0.5, value = metric.Value.Mean },
                                                   new Quantile() { quantile = 0.75, value = metric.Value.Percentile75 },
                                                   new Quantile() { quantile = 0.95, value = metric.Value.Percentile95 },
                                                   // new Quantile(){quantile = 0.98, value = metric.Value.Percentile98},
                                                   new Quantile() { quantile = 0.99, value = metric.Value.Percentile99 },
                                                   // new Quantile(){quantile = 0.999, value = metric.Value.Percentile999}
                                               }
                                           },
                                 label = metric.Tags.ToLabelPairs()
                             }
                         };

            return result;
        }

        public static List<Metric> ToPrometheusMetrics(this TimerValueSource metric)
        {
            // Prometheus advocates always using seconds as a base unit for time
            var rescaledVal = metric.Value.Scale(TimeUnit.Seconds, TimeUnit.Seconds);
            var result = new List<Metric>
                         {
                             new Metric()
                             {
                                 summary = new Summary()
                                           {
                                               sample_count = (ulong)rescaledVal.Histogram.Count,
                                               sample_sum = rescaledVal.Histogram.Sum,
                                               quantile =
                                               {
                                                   new Quantile() { quantile = 0.5, value = rescaledVal.Histogram.Mean },
                                                   new Quantile() { quantile = 0.75, value = rescaledVal.Histogram.Percentile75 },
                                                   new Quantile() { quantile = 0.95, value = rescaledVal.Histogram.Percentile95 },
                                                   // new Quantile(){quantile = 0.98, value = metric.Value.Histogram.Percentile98},
                                                   new Quantile() { quantile = 0.99, value = rescaledVal.Histogram.Percentile99 },
                                                   // new Quantile(){quantile = 0.999, value = metric.Value.Histogram.Percentile999}
                                               }
                                           },
                                 label = metric.Tags.ToLabelPairs()
                             }
                         };

            return result;
        }

        private static string FormatName(string name) { return Rgx.Replace(name, "_").ToLower(); }
    }
}