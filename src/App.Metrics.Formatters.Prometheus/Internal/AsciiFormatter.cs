// <copyright file="AsciiFormatter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace App.Metrics.Formatters.Prometheus.Internal
{
    internal static class AsciiFormatter
    {
        public static void Format(Stream destination, IEnumerable<MetricFamily> metrics)
        {
            var metricFamilys = metrics.ToArray();
            using (var streamWriter = new StreamWriter(destination, Encoding.UTF8))
            {
                streamWriter.NewLine = "\n";
                foreach (var metricFamily in metricFamilys)
                {
                    WriteFamily(streamWriter, metricFamily);
                }
            }
        }

        internal static string Format(IEnumerable<MetricFamily> metrics, NewLineFormat newLine)
        {
            var newLineChar = GetNewLineChar(newLine);
            var metricFamilys = metrics.ToArray();
            var s = new StringBuilder();
            foreach (var metricFamily in metricFamilys)
            {
                s.Append(WriteFamily(metricFamily, newLineChar));
            }

            return s.ToString();
        }

        private static void WriteFamily(StreamWriter streamWriter, MetricFamily metricFamily)
        {
            streamWriter.WriteLine("# HELP {0} {1}", metricFamily.name, metricFamily.help);
            streamWriter.WriteLine("# TYPE {0} {1}", metricFamily.name, metricFamily.type.ToString().ToLowerInvariant());
            foreach (var metric in metricFamily.metric)
            {
                WriteMetric(streamWriter, metricFamily, metric);
            }
        }

        private static string WriteFamily(MetricFamily metricFamily, string newLine)
        {
            var s = new StringBuilder();
            s.Append(string.Format("# HELP {0} {1}", metricFamily.name, metricFamily.help), newLine);
            s.Append(string.Format("# TYPE {0} {1}", metricFamily.name, metricFamily.type.ToString().ToLowerInvariant()), newLine);
            foreach (var metric in metricFamily.metric)
            {
                s.Append(WriteMetric(metricFamily, metric, newLine), newLine);
            }

            return s.ToString();
        }

        private static void WriteMetric(StreamWriter streamWriter, MetricFamily family, Metric metric)
        {
            var familyName = family.name;

            if (metric.gauge != null)
            {
                streamWriter.WriteLine(SimpleValue(familyName, metric.gauge.value, metric.label));
            }
            else if (metric.counter != null)
            {
                streamWriter.WriteLine(SimpleValue(familyName, metric.counter.value, metric.label));
            }
            else if (metric.summary != null)
            {
                streamWriter.WriteLine(SimpleValue(familyName, metric.summary.sample_sum, metric.label, "_sum"));
                streamWriter.WriteLine(SimpleValue(familyName, metric.summary.sample_count, metric.label, "_count"));

                foreach (var quantileValuePair in metric.summary.quantile)
                {
                    var quantile = double.IsPositiveInfinity(quantileValuePair.quantile)
                        ? "+Inf"
                        : quantileValuePair.quantile.ToString(CultureInfo.InvariantCulture);
                    streamWriter.WriteLine(
                        SimpleValue(
                            familyName,
                            quantileValuePair.value,
                            metric.label.Concat(new[] { new LabelPair { name = "quantile", value = quantile } })));
                }
            }
            else if (metric.histogram != null)
            {
                streamWriter.WriteLine(SimpleValue(familyName, metric.histogram.sample_sum, metric.label, "_sum"));
                streamWriter.WriteLine(SimpleValue(familyName, metric.histogram.sample_count, metric.label, "_count"));
                foreach (var bucket in metric.histogram.bucket)
                {
                    var value = double.IsPositiveInfinity(bucket.upper_bound) ? "+Inf" : bucket.upper_bound.ToString(CultureInfo.InvariantCulture);
                    streamWriter.WriteLine(
                        SimpleValue(
                            familyName,
                            bucket.cumulative_count,
                            metric.label.Concat(new[] { new LabelPair { name = "le", value = value } }),
                            "_bucket"));
                }
            }
            else
            {
                // not supported
            }
        }

        private static string WriteMetric(MetricFamily family, Metric metric, string newLine)
        {
            var s = new StringBuilder();
            var familyName = family.name;

            if (metric.gauge != null)
            {
                s.Append(SimpleValue(familyName, metric.gauge.value, metric.label), newLine);
            }
            else if (metric.counter != null)
            {
                s.Append(SimpleValue(familyName, metric.counter.value, metric.label), newLine);
            }
            else if (metric.summary != null)
            {
                s.Append(SimpleValue(familyName, metric.summary.sample_sum, metric.label, "_sum"), newLine);
                s.Append(SimpleValue(familyName, metric.summary.sample_count, metric.label, "_count"), newLine);

                foreach (var quantileValuePair in metric.summary.quantile)
                {
                    var quantile = double.IsPositiveInfinity(quantileValuePair.quantile)
                        ? "+Inf"
                        : quantileValuePair.quantile.ToString(CultureInfo.InvariantCulture);
                    s.Append(
                        SimpleValue(
                            familyName,
                            quantileValuePair.value,
                            metric.label.Concat(new[] { new LabelPair { name = "quantile", value = quantile } })), newLine);
                }
            }
            else if (metric.histogram != null)
            {
                s.Append(SimpleValue(familyName, metric.histogram.sample_sum, metric.label, "_sum"), newLine);
                s.Append(SimpleValue(familyName, metric.histogram.sample_count, metric.label, "_count"), newLine);
                foreach (var bucket in metric.histogram.bucket)
                {
                    var value = double.IsPositiveInfinity(bucket.upper_bound) ? "+Inf" : bucket.upper_bound.ToString(CultureInfo.InvariantCulture);
                    s.Append(
                        SimpleValue(
                            familyName,
                            bucket.cumulative_count,
                            metric.label.Concat(new[] { new LabelPair { name = "le", value = value } }),
                            "_bucket"), newLine);
                }
            }
            else
            {
                // not supported
            }

            return s.ToString();
        }

        private static string WithLabels(string familyName, IEnumerable<LabelPair> labels)
        {
            var labelPairs = labels as LabelPair[] ?? labels.ToArray();

            if (labelPairs.Length == 0)
            {
                return familyName;
            }

            return string.Format("{0}{{{1}}}", familyName, string.Join(",", labelPairs.Select(l => string.Format("{0}=\"{1}\"", l.name, l.value))));
        }

        private static string SimpleValue(string family, double value, IEnumerable<LabelPair> labels, string namePostfix = null)
        {
            return string.Format("{0} {1}", WithLabels(family + (namePostfix ?? string.Empty), labels), value.ToString(CultureInfo.InvariantCulture));
        }

        private static string GetNewLineChar(NewLineFormat newLine)
        {
            switch (newLine)
            {
                case NewLineFormat.Auto:
                    return Environment.NewLine;
                case NewLineFormat.Windows:
                    return "\r\n";
                case NewLineFormat.Unix:
                case NewLineFormat.Default:
                    return "\n";
                default:
                    throw new ArgumentOutOfRangeException(nameof(newLine), newLine, null);
            }
        }

        private static void Append(this StringBuilder sb, string line, string newLineChar)
        {
            sb.Append(line + newLineChar);
        }
    }
}
