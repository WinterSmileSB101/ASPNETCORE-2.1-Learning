using helloJob.JobServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace helloJob
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var host = BuildJobHost();
            host.Run();
        }

        private static IHost BuildJobHost()
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration(config =>
                {
                    // Microsoft.Extensions.Configuration
                    config.SetBasePath(Directory.GetCurrentDirectory());

                    config.AddEnvironmentVariables("ASPNETCORE_");
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddJsonFile("appsettings.json", true);
                    configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", true);
                    configApp.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();
                    services.AddHostedService<HelloJobService>();
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    // Microsoft.Extensions.Logging
                    configLogging.AddConsole();
                    if (hostContext.HostingEnvironment.EnvironmentName == EnvironmentName.Development) {
                        // Microsoft.Extensions.Logging
                        configLogging.AddDebug();
                    }
                })
                .UseConsoleLifetime()
                .Build();
            return host;
        }
    }
}
