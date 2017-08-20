// <copyright file="Startup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using MetricsPrometheusSandboxMvc.JustForTesting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MetricsPrometheusSandboxMvc
{
    public class Startup
    {
        private static readonly bool HaveAppRunSampleRequests = true;

        public Startup(IConfiguration configuration) { Configuration = configuration; }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IApplicationLifetime lifetime)
        {
            app.UseMvc();

            if (HaveAppRunSampleRequests)
            {
                SampleRequests.Run(lifetime.ApplicationStopping);
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTestStuff();

            services.AddMvc(options => options.AddMetricsResourceFilter());
        }
    }
}