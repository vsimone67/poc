using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ApiGateway.Extensions;
using Serilog;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateway
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsPolicy();
            services.AddMvcExtensions();
            services.AddJwtAuthentication();
            services.AddCommandQueryHandlers();
            services.AddMappingProfiles();
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
            services.ConfigureDiEnvironment(Configuration);
            services.MapConfigToClass(Configuration);
            //services.AddMassTransitRabbitMq();
            services.AddOcelot();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UserCorsPolicy();
            app.UseJwtAuthentication();
            app.UseMvcExtensions();

            app.UseOcelot().Wait();
            //app.UseHttpsRedirection();
        }
    }
}
