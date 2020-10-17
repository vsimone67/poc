using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ApiGateway.Extensions;
using Serilog;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Pacco.APIGateway.Ocelot.Infrastructure;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;

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

            //services.AddSingleton<IAnonymousRouteValidator, AnonymousRouteValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            // var configuration = new OcelotPipelineConfiguration
            // {
            //     AuthenticationMiddleware = async (context, next) =>
            //     {
            //         if (!context.DownstreamReRoute.IsAuthenticated)
            //         {
            //             await next.Invoke();
            //             return;
            //         }

            //         if (context.HttpContext.RequestServices.GetRequiredService<IAnonymousRouteValidator>()
            //             .HasAccess(context.HttpContext.Request.Path))
            //         {
            //             await next.Invoke();
            //             return;
            //         }

            //         var authenticateResult = await context.HttpContext.AuthenticateAsync();
            //         if (authenticateResult.Succeeded)
            //         {
            //             context.HttpContext.User = authenticateResult.Principal;
            //             await next.Invoke();
            //             return;
            //         }

            //         context.Errors.Add(new UnauthenticatedError("Unauthenticated"));
            //     }
            // };


            app.UseRouting();
            app.UserCorsPolicy();
            app.UseJwtAuthentication();
            app.UseMvcExtensions();

            app.UseOcelot().Wait();
            //app.UseHttpsRedirection();
        }
    }
}
