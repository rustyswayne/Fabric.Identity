﻿using System.IO;
using Fabric.Identity.API.Extensions;
using Microsoft.AspNetCore.Hosting;

namespace Fabric.Identity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var appConfig = new Configuration.ConfigurationProvider().GetAppConfiguration(Directory.GetCurrentDirectory());

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .UseIisIntegrationIfConfigured(appConfig)
                .UseUrls("http://*:5001")
                .Build();

            host.Run();
        }
    }
}
