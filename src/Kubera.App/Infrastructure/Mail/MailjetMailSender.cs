using Kubera.General;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Net.Http.Headers;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kubera.App.Infrastructure.Mail
{
    public class MailjetMailSender : IEmailSender
    {
        private const string _mailSenderAuthScheme = "Basic";

        private readonly HttpClient _httpClient;
        private readonly IAppSettings _appSettings;
        private readonly static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        };

        public MailjetMailSender(HttpClient httpClient, IAppSettings appSettings)
        {
            _httpClient = httpClient;
            _appSettings = appSettings;

            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(HeaderNames.ContentType, MediaTypeNames.Application.Json);
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(HeaderNames.Accept, MediaTypeNames.Application.Json);
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using var content = CreateBodyContent(email, subject, htmlMessage);
            using var request = CreateRequest(content);

            using var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }

        private HttpContent CreateBodyContent(string email, string subject, string htmlMessage)
        {
            var body = MailjetRequest.Create(email, subject, htmlMessage, _appSettings);
            var content = new StringContent(JsonSerializer.Serialize(body, _jsonSerializerOptions), Encoding.UTF8, MediaTypeNames.Application.Json);

            return content;
        }

        private HttpRequestMessage CreateRequest(HttpContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _appSettings.Mail.Api);
            var byteArray = Encoding.ASCII.GetBytes(_appSettings.Mail.ApiKey);

            request.Headers.Authorization = new AuthenticationHeaderValue(_mailSenderAuthScheme, Convert.ToBase64String(byteArray));
            request.Content = content;

            return request;
        }
    }
}
