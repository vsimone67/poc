using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using System;
using GreenPipes;
using Hub.Service.Extensions.MassTransit.Consumers;
using MassTransit.ActiveMqTransport;

namespace Hub.Service.Extensions
{
    public static class MassTransitExtention
    {
        public static IServiceCollection AddMassTransitRabbitMq(this IServiceCollection services)
        {
            //https://masstransit-project.com/getting-started/
            services.AddMassTransit(x =>
              {
                  ServiceBusSettings serviceBusSettings;
                  using (var serviceProvider = services.BuildServiceProvider())
                  {
                      var configuration = serviceProvider.GetService<IConfiguration>();
                      services.Configure<ServiceBusSettings>(configuration.GetSection("ServiceBusSettings"));
                      serviceBusSettings = configuration.GetOptions<ServiceBusSettings>("ServiceBusSettings");
                  }
                  x.AddConsumer<MibCompletedConsumer>();  // this is the way to subscribe to events/commands

                  x.UsingRabbitMq((context, cfg) =>
                  {
                      cfg.Host(new Uri($"rabbitmq://{serviceBusSettings.ServerName}"), h =>
                      {
                          h.Username(serviceBusSettings.UserName);
                          h.Password(serviceBusSettings.Password);
                      });
                      cfg.ReceiveEndpoint(serviceBusSettings.ListenQueueName, e => // this is the way to subscribe to events/commands
                      {

                          e.Consumer<MibCompletedConsumer>(context);
                          e.UseMessageRetry(r =>
                          {
                              r.Immediate(serviceBusSettings.NumberOfRetries);
                              r.Intervals(serviceBusSettings.RetryInterval);
                          });

                      });

                  });
              });

            services.AddMassTransitHostedService();
            return services;
        }

        public static IServiceCollection AddMassTransitActiveMq(this IServiceCollection services)
        {
            //https://masstransit-project.com/getting-started/
            services.AddMassTransit(x =>
              {
                  ServiceBusSettings serviceBusSettings;
                  using (var serviceProvider = services.BuildServiceProvider())
                  {
                      var configuration = serviceProvider.GetService<IConfiguration>();
                      services.Configure<ServiceBusSettings>(configuration.GetSection("ServiceBusSettings"));
                      serviceBusSettings = configuration.GetOptions<ServiceBusSettings>("ServiceBusSettings");
                  }
                  x.AddConsumer<MibCompletedConsumer>(); // this is the way to subscribe to events/commands

                  x.UsingActiveMq((context, cfg) =>
                  {
                      cfg.Host(new Uri($"activemq://{serviceBusSettings.ServerName}"), h =>
                      {
                          // h.UseSsl();

                          //h.Username("admin");
                          //h.Password("admin");
                      });
                      cfg.ReceiveEndpoint(serviceBusSettings.ListenQueueName, e => // this is the way to subscribe to events/commands
                      {

                          e.Consumer<MibCompletedConsumer>(context);
                          e.UseMessageRetry(r =>
                          {
                              r.Immediate(serviceBusSettings.NumberOfRetries);
                              r.Intervals(serviceBusSettings.RetryInterval);
                          });

                      });

                  });
              });

            services.AddMassTransitHostedService();
            return services;
        }
    }



}