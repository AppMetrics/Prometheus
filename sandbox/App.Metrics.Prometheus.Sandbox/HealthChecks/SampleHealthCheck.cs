// <copyright file="SampleHealthCheck.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Sandbox.HealthChecks
{
    public class SampleHealthCheck : HealthCheck
    {
        public SampleHealthCheck()
            : base("Random Health Check")
        {
        }

        /// <inheritdoc />
        protected override ValueTask<HealthCheckResult> CheckAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (DateTime.UtcNow.Second <= 20)
            {
                return new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded());
            }

            if (DateTime.UtcNow.Second >= 40)
            {
                return new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy());
            }

            return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy());
        }
    }
}
