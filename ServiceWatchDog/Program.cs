using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using My.ServiceWatchdog.Extensions;
using Serilog;

namespace serviceWatchdog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var basePath = Environment.GetEnvironmentVariable("appdirectory").NullToEmpty();

                var configuration = new ConfigurationBuilder()
                    .AddJsonFile($"{basePath}appsettings.json")
                    .Build();


                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .AddConfiguration()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
