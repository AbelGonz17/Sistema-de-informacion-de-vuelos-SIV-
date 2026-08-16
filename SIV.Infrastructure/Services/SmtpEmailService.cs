using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using SIV.Application.Common.Interfaces;
using System;
using System.Threading.Tasks;

namespace SIV.Infrastructure.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SmtpEmailService> _logger;

        public SmtpEmailService(IConfiguration configuration, ILogger<SmtpEmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var host = _configuration["EmailSettings:Host"] ?? "smtp.gmail.com";
            var port = int.Parse(_configuration["EmailSettings:Port"] ?? "587");
            var user = _configuration["EmailSettings:User"] ?? "";
            var pass = _configuration["EmailSettings:Password"] ?? "";

            try
            {
                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse(user));
                message.To.Add(MailboxAddress.Parse(to));
                message.Subject = subject;
                message.Body = new TextPart("html") { Text = body };

                using var client = new SmtpClient();

                // Conectar usando StartTls (puerto 587) o SslOnConnect (puerto 465)
                var secureOption = port == 465
                    ? SecureSocketOptions.SslOnConnect
                    : SecureSocketOptions.StartTls;

                await client.ConnectAsync(host, port, secureOption);
                await client.AuthenticateAsync(user, pass);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation($"Correo enviado exitosamente a {to}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enviando correo a {to}");
                throw;
            }
        }
    }
}
