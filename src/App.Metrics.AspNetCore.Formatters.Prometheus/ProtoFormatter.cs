// <copyright file="ProtoFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;

namespace App.Metrics.Formatters.Prometheus
{
    internal static class ProtoFormatter
    {
        public static byte[] Format(IEnumerable<MetricFamily> metrics)
        {
            using (var memoryStream = new MemoryStream())
            {
                Format(memoryStream, metrics);
                return memoryStream.ToArray();
            }
        }

        public static void Format(Stream destination, IEnumerable<MetricFamily> metrics)
        {
            var metricFamilys = metrics.ToArray();
            foreach (var metricFamily in metricFamilys)
            {
                Serializer.SerializeWithLengthPrefix(destination, metricFamily, PrefixStyle.Base128, 0);
            }
        }
    }
}