using Kubera.General.Settings;

namespace Kubera.App.Infrastructure.Mail
{
    public class MailOptions : IMailOptions
    {
        public string Api { get; set; }

        public string ApiKey { get; set; }

        public string FromName { get; set; }

        public string FromEmail { get; set; }

        public string SenderName { get; set; }

        public string SenderEmail { get; set; }
    }
}
