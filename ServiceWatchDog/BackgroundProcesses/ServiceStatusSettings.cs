using System.Collections.Generic;

namespace ServiceWatchDog.BackgroundProcesses
{
    public class ServiceStatusSettings
    {
        public List<ServiceInfo> Services { get; set; }
        public int WakeupInterval { get; set; }
        public string SmtpServer { get; set; }
        public string EmailToList { get; set; }
        public string EmailFromUser { get; set; }
        public bool SendEmailOnError { get; set; }
        public string EmailSubject { get; set; }

    }
}
