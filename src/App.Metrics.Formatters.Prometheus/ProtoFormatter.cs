// <copyright file="ProtoFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Metrics.Extensions.Prometheus.DataContracts;
using ProtoBuf;

namespace App.Metrics.Extensions.Reporting.Prometheus
{
    internal class ProtoFormatter
    {
        public static byte[] Format(IEnumerable<MetricFamily> metrics)
        {
            using (MemoryStream memoryStream = new MemoryStream())
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