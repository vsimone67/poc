using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceWatchDog.BackgroundProcesses;

namespace My.ServiceWatchdog.Extensions
{
    public static class ConfigurationExtension
    {

        public static IServiceCollection MapConfigToClass(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServiceStatusSettings>(options => configuration.GetSection("ServiceStatus").Bind(options));
            return services;
        }
        public static IHostBuilder AddConfiguration(this IHostBuilder builder, string basePath = "")
        {
            builder.ConfigureAppConfiguration((builderContext, config) =>
            {
                var env = builderContext.HostingEnvironment;

                if (basePath != string.Empty)
                    config.SetBasePath(basePath);

                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                // we are sharing the appsettings file between angular and .net core so use the one in the angular folder
                config.AddJsonFile($"ClientApp/Dist/assets/app-settings/appsettings.site.json", optional: false, reloadOnChange: true);
                config.AddEnvironmentVariables();
            });

            return builder;
        }

    }
}