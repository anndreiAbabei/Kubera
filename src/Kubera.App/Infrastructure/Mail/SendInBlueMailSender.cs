using Kubera.General;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Net.Http.Headers;
using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kubera.App.Infrastructure.Mail
{
    public class SendInBlueMailSender : IEmailSender
    {
        private const string AppKeyHeaderName = "api-key";

        private readonly HttpClient _httpClient;
        private readonly IAppSettings _appSettings;

        public SendInBlueMailSender(HttpClient httpClient, IAppSettings appSettings)
        {
            _httpClient = httpClient;
            _appSettings = appSettings;

            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(HeaderNames.ContentType, MediaTypeNames.Application.Json);
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(HeaderNames.Accept, MediaTypeNames.Application.Json);
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(AppKeyHeaderName, appSettings.Mail.ApiKey);
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var request = new SendInBlueMailRequest(email, subject, htmlMessage, _appSettings);
            using var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json);
            using var response = await _httpClient.PostAsync(_appSettings.Mail.Api, content);

            response.EnsureSuccessStatusCode();
        }
    }
}
