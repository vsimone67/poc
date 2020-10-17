using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pacco.APIGateway.Ocelot.Infrastructure;

namespace ApiGateway.Extensions
{
    public static class ConfigurationExtension
    {

        public static IServiceCollection MapConfigToClass(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AnonymousRoutesOptions>(configuration.GetSection("AnonymousRoutes"));
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

    }
}