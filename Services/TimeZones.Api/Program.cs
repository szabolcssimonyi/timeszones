using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TimeZones.Domain.Contexts;
using TimeZones.Extensibility.Interfaces;

namespace TimeZones.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            try
            {
                DoSeed(host);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var webHostBuilder = WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    IHostingEnvironment env = context.HostingEnvironment;
                    builder
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables();
                });
            IWebHost host = webHostBuilder.UseStartup<Startup>().Build();
            return host;
        }

        private async static void DoSeed(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var timezoneContext = scope.ServiceProvider.GetRequiredService<TimeZonesDbContext>();
                appContext.Database.Migrate();
                timezoneContext.Database.Migrate();
                var seeders = scope.ServiceProvider.GetRequiredService<IEnumerable<IDomainSeeder>>();
                foreach (var seeder in seeders)
                {
                    await seeder.SeedAsync();
                }
            }
        }
    }
}
