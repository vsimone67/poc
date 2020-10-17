using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace ServiceWatchDog.BackgroundProcesses
{
    public class ServicesStatusMonitor : BackgroundService
    {
        private readonly ILogger<ServicesStatusMonitor> _logger;
        private readonly ServiceStatusSettings _settings;

        public ServicesStatusMonitor(IOptions<ServiceStatusSettings> settings, ILogger<ServicesStatusMonitor> logger)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"ServicesStatusMonitor is starting.");

            stoppingToken.Register(() => _logger.LogDebug($" ServicesStatusMonitor background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug($"ServicesStatusMonitor task checking services, count is: {_settings.Services.Count}");

                await CheckServiceStatus();

                _logger.LogDebug($"Sleeping for {_settings.WakeupInterval} ms");
                await Task.Delay(_settings.WakeupInterval, stoppingToken);
            }

            _logger.LogDebug($"ServicesStatusMonitor background task is stopping.");
            SendEmail("ServicesStatusMonitor background task has stopped");
        }

        private async Task CheckServiceStatus()
        {
            foreach (var service in _settings.Services)
            {
                // webreqeust will not return error code back, it will throw and exception so we need to put a try/catch/finally so we can iterate through the entire service monitor list
                try
                {
                    var response = await HttpHelper.GetWebRequest(service.Uri);  //  Call health check

                    if (response.StatusCode != System.Net.HttpStatusCode.OK) // if we do not get a OK status log error and send email
                    {
                        SendServiceDownMessage(service, $"Status code returned {response.StatusCode}");  // log error and send email to team that service is down
                    }
                    response.Close();  // close connection
                }
                catch (Exception ex)
                {
                    SendServiceDownMessage(service, ex.Message);  // we did not get a response back (either site is stopped or an internal error is returned), this is a service down
                }
                finally
                {
                }
            }
        }
        private void SendServiceDownMessage(ServiceInfo service, string serverErrorMessage)
        {
            string errorMessage = $"Service {service.Name}, Uri {service.Uri}, is not responding.  Server Error: {serverErrorMessage}";
            _logger.LogError(errorMessage);  // log error to sink
            SendEmail(errorMessage);  // send email to user list
        }

        private void SendEmail(string message)
        {
            if (_settings.SendEmailOnError)  // check to see if we want to send emails on error 
            {
                SmtpClient client = new SmtpClient(_settings.SmtpServer);

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_settings.EmailFromUser);
                mailMessage.To.Add(_settings.EmailToList);
                mailMessage.Body = message;
                mailMessage.Subject = _settings.EmailSubject;
                client.Send(mailMessage);
            }

        }


    }
}
