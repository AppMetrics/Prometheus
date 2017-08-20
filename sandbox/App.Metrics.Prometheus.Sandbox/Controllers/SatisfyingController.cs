// <copyright file="SatisfyingController.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Prometheus.Sandbox.JustForTesting;
using Microsoft.AspNetCore.Mvc;

namespace App.Metrics.Prometheus.Sandbox.Controllers
{
    [Route("api/[controller]")]
    public class SatisfyingController : Controller
    {
        private static readonly Random Rnd = new Random();
        private readonly RequestDurationForApdexTesting _durationForApdexTesting;
        private readonly IMetrics _metrics;

        public SatisfyingController(IMetrics metrics, RequestDurationForApdexTesting durationForApdexTesting)
        {
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
            _durationForApdexTesting = durationForApdexTesting;
        }

        [HttpGet]
        public async Task<int> Get()
        {
            var duration = _durationForApdexTesting.NextSatisfiedDuration;

            await Task.Delay(duration, HttpContext.RequestAborted);

            return duration;
        }
    }
}