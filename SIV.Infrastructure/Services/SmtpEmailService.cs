using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SIV.Application.Common.Interfaces;

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
            try
            {
                var host = _configuration["EmailSettings:Host"] ?? "smtp.example.com";
                var port = int.Parse(_configuration["EmailSettings:Port"] ?? "587");
                var user = _configuration["EmailSettings:User"] ?? "user@example.com";
                var pass = _configuration["EmailSettings:Password"] ?? "password";

                using var client = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(user, pass),
                    EnableSsl = true,
                    Timeout = 5000
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(user),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(to);

                await client.SendMailAsync(mailMessage);
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
