using Kubera.General;
using Kubera.General.Extensions;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kubera.App.Infrastructure.Mail
{
    internal class SendInBlueMailRequest
    {
        [JsonPropertyName("sender")]
        public SendInBlueMailRequestEmail Sender { get; set; }

        [JsonPropertyName("to")]
        public IEnumerable<SendInBlueMailRequestEmail> To { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; }

        [JsonPropertyName("htmlContent")]
        public string HtmlMessage { get; }

        public SendInBlueMailRequest()
        {

        }

        public SendInBlueMailRequest(string email, string subject, string htmlMessage, IAppSettings appSettings)
            : this()
        {
            Sender = new SendInBlueMailRequestEmail
            {
                Email = appSettings.Mail.SenderEmail,
                Name = appSettings.Mail.SenderName
            }; 
            To = new SendInBlueMailRequestEmail
            {
                Email = email,
                Name = appSettings.AppName
            }.Yield();

            Subject = subject;
            HtmlMessage = htmlMessage;
        }
    }

    public class SendInBlueMailRequestEmail
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("sender")]
        public string Email { get; set; }
    }
}