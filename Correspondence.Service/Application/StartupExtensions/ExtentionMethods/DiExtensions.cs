﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Correspondence.Service.Persistence.DbService;
using Correspondence.Service.Persistence.Context;

namespace Correspondence.Service.Extensions
{
    public static class DiExtensions
    {
        public static IServiceCollection ConfigureDiEnvironment(this IServiceCollection services, IConfiguration Configuration)
        {
            // ******* Add Database Services here *******
            services.AddTransient<IDatabaseService, DatabaseService>();

            // ************** Add Contexts here **********       
            //Uncomment and change to the DB Context you are using
            //services.AddDbContext<DBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Correspondence.Service"), providerOptions => providerOptions.CommandTimeout(120)));

            // ***** Add remaining services here **************
            //services.AddHostedService<ServicesStatusMonitor>(); // if you want a background service then unncomment this line and use the background class you created
            return services;
        }
    }
}