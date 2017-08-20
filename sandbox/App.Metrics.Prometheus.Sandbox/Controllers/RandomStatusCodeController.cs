// <copyright file="RandomStatusCodeController.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Prometheus.Sandbox.JustForTesting;
using Microsoft.AspNetCore.Mvc;

namespace App.Metrics.Prometheus.Sandbox.Controllers
{
    [Route("api/[controller]")]
    public class RandomStatusCodeController : Controller
    {
        private readonly RandomStatusCodeForTesting _randomStatusCodeForTesting;
        private readonly IMetrics _metrics;

        public RandomStatusCodeController(IMetrics metrics, RandomStatusCodeForTesting randomStatusCodeForTesting)
        {
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
            _randomStatusCodeForTesting = randomStatusCodeForTesting;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(_randomStatusCodeForTesting.NextStatusCode);
        }
    }
}
