// <copyright file="MetricsPrometheusPushGatewayReporterSettings.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics.Formatters.Prometheus
{
    public class MetricsPrometheusPushGatewayReporterSettings
    {
        public string Endpoint { get; set; }

        public string Job { get; set; }

        public string Instance { get; set; }

        public IEnumerable<KeyValuePair<string, string>> AdditionalLabels { get; set; }
    }
}