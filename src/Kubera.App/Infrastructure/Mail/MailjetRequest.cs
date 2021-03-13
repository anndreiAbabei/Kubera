using Kubera.General;
using Kubera.General.Extensions;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kubera.App.Infrastructure.Mail
{
    internal class MailjetRequest
    {
        [JsonPropertyName("Messages")]
        public IEnumerable<MailjetMessage> Messages { get; set; }

        public static MailjetRequest Create(string email, string subject, string htmlMessage, IAppSettings appSettings)
        {
            var message = new MailjetMessage
            {
                From = MailjetContact.Create(appSettings.Mail.FromEmail, appSettings.Mail.FromName),
                To = MailjetContact.Create(email),
                Subject = subject,
                HTMLPart = htmlMessage
            };

            if (!string.IsNullOrEmpty(appSettings.Mail.SenderEmail))
                message.Sender = MailjetContact.Create(appSettings.Mail.SenderEmail, appSettings.Mail.SenderName);

            return new MailjetRequest
            {

                Messages = message.Yield()
            };
        }

        public class MailjetMessage
        {
            [JsonPropertyName("From")]
            public MailjetContact From { get; set; }

            [JsonPropertyName("Sender")]
            public MailjetContact Sender { get; set; }

            [JsonPropertyName("To")]
            public MailjetContact To { get; set; }

            [JsonPropertyName("Subject")]
            public string Subject { get; set; }

            [JsonPropertyName("TextPart")]
            public string TextPart { get; set; }

            [JsonPropertyName("HTMLPart")]
            public string HTMLPart { get; set; }

            [JsonPropertyName("CustomID")]
            public string CustomID { get; set; }
        }

        public class MailjetContact
        {
            [JsonPropertyName("Email")]
            public string Email { get; set; }

            [JsonPropertyName("Name")]
            public string Name { get; set; }

            internal static MailjetContact Create(string email, string name = null)
            {
                return new MailjetContact
                {
                    Email = email,
                    Name = name
                };
            }
        }
    }

}
class a { 

string str = @"{
  ""Messages"":[
    {
                ""From"": {
                    ""Email"": ""andrei@stgo.live"",
        ""Name"": ""Andrei""
                },
      ""To"": [
        {
                    ""Email"": ""andrei@stgo.live"",
          ""Name"": ""Andrei""
        }
      ],
      ""Subject"": ""My first Mailjet email"",
      ""TextPart"": ""Greetings from Mailjet."",
      ""HTMLPart"": ""<h3>Dear passenger 1, welcome to <a href='https://www.mailjet.com/'>Mailjet</a>!</h3><br />May the delivery force be with you!"",
      ""CustomID"": ""AppGettingStartedTest""
    }
  ]
}";
}