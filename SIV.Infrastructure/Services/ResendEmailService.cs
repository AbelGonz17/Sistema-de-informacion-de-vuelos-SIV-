using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SIV.Application.Common.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SIV.Infrastructure.Services
{
    public class ResendEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ResendEmailService> _logger;
        private readonly HttpClient _httpClient;

        public ResendEmailService(IConfiguration configuration, ILogger<ResendEmailService> logger, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("Resend");
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var apiKey = _configuration["EmailSettings:ResendApiKey"];
            var from = _configuration["EmailSettings:From"] ?? "onboarding@resend.dev";

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                _logger.LogWarning("[Email] ResendApiKey no configurado. El correo no será enviado.");
                return;
            }

            var payload = new
            {
                from = from,
                to = new[] { to },
                subject = subject,
                html = body
            };

            var json = JsonSerializer.Serialize(payload);
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.resend.com/emails")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            _logger.LogInformation($"[Email] Enviando correo via Resend a {to} con asunto: {subject}");

            var response = await _httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"[Email] Correo enviado exitosamente a {to}. Response: {responseBody}");
            }
            else
            {
                _logger.LogError($"[Email] Error al enviar correo a {to}. Status: {response.StatusCode}. Body: {responseBody}");
                throw new Exception($"Resend API error: {response.StatusCode} - {responseBody}");
            }
        }
    }
}
