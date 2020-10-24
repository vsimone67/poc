using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using System;
using GreenPipes;
using Fac.Service.Infrastructure.MassTransit.Models;
using Correspondence.Service.Extensions.MassTransit.Consumers;
using Correspondence.Service.Infrastructure.MassTransit.Models;
using MassTransit.ActiveMqTransport;
namespace Correspondence.Service.Extensions
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
                  x.AddConsumer<DecisionMadeConsumer>();

                  x.UsingRabbitMq((context, cfg) =>
                  {
                      cfg.Host(new Uri($"rabbitmq://{serviceBusSettings.ServerName}"), h =>
                      {
                          h.Username(serviceBusSettings.UserName);
                          h.Password(serviceBusSettings.Password);
                      });
                      cfg.ReceiveEndpoint(serviceBusSettings.ListenQueueName, e =>
                      {

                          e.Consumer<DecisionMadeConsumer>(context);
                          e.UseMessageRetry(r =>
                          {
                              r.Immediate(serviceBusSettings.NumberOfRetries);
                              r.Intervals(serviceBusSettings.RetryInterval);
                          });

                      });

                      EndpointConvention.Map<FacDecisionEvent>(new Uri($"rabbitmq://{serviceBusSettings.ServerName}/{serviceBusSettings.SubmitQueueName}"));

                  });
              });

            services.AddMassTransitHostedService();
            return services;
        }

        public static IServiceCollection AddMassTransitActiveMq(this IServiceCollection services)
        {

            services.AddMassTransit(x =>
              {
                  ServiceBusSettings serviceBusSettings;
                  using (var serviceProvider = services.BuildServiceProvider())
                  {
                      var configuration = serviceProvider.GetService<IConfiguration>();
                      services.Configure<ServiceBusSettings>(configuration.GetSection("ServiceBusSettings"));
                      serviceBusSettings = configuration.GetOptions<ServiceBusSettings>("ServiceBusSettings");
                  }
                  x.AddConsumer<DecisionMadeConsumer>();
                  x.UsingActiveMq((context, cfg) =>
                  {
                      cfg.Host(new Uri($"activemq://{serviceBusSettings.ServerName}"), h =>
                      {
                          // h.UseSsl();
                          //h.Username("admin");
                          //h.Password("admin");
                      });
                      cfg.ReceiveEndpoint(serviceBusSettings.ListenQueueName, e =>
                     {

                         e.Consumer<DecisionMadeConsumer>(context);
                         e.UseMessageRetry(r =>
                         {
                             r.Immediate(serviceBusSettings.NumberOfRetries);
                             r.Intervals(serviceBusSettings.RetryInterval);
                         });

                     });

                      // the is the way to setup publising events
                      EndpointConvention.Map<FacDecisionEvent>(new Uri($"activemq://{serviceBusSettings.ServerName}/{serviceBusSettings.SubmitQueueName}"));
                  });
              });

            services.AddMassTransitHostedService();
            return services;
        }
    }



}