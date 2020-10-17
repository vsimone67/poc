using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Authentication.Service.Models;

namespace Authentication.Service.Extensions
{
    public static class ConfigurationExtension
    {

        public static IServiceCollection MapConfigToClass(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<AuthorizationSettings>(configuration.GetSection("Authorization"));
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
                config.AddJsonFile("appsettings.secrets.json", optional: true, reloadOnChange: true);
                config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                config.AddEnvironmentVariables();
            });

            return builder;
        }

        public static IHostBuilder AddAppConfigurationFromEnvironment(this IHostBuilder builder)
        {
            var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                                                               .AddEnvironmentVariables()
                                                               .Build(); // get variables from environment to pass to config (if exist)

            string basePath = config.GetValue<string>("appdirectory").NullToEmpty();

            return AddConfiguration(builder, basePath);
        }

    }
}