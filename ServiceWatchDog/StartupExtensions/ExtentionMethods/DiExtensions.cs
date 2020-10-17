using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceWatchDog.BackgroundProcesses;

namespace My.ServiceWatchdog.Extensions
{
    public static class DiExtensions
    {
        public static IServiceCollection ConfigureDiEnvironment(this IServiceCollection services, IConfiguration Configuration)
        {
            // ******* Add Database Services here *******



            // ************** Add Contexts here **********          



            // ***** Add remaining services here **************
            //services.AddHostedService<ServicesStatusMonitor>(); // background process to monitor services

            return services;
        }
    }
}