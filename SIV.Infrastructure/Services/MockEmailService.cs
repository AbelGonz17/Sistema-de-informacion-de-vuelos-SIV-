using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SIV.Application.Common.Interfaces;

namespace SIV.Infrastructure.Services
{
    public class MockEmailService : IEmailService
    {
        private readonly ILogger<MockEmailService> _logger;

        public MockEmailService(ILogger<MockEmailService> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string to, string subject, string body)
        {
            _logger.LogInformation("--- MOCK EMAIL ENVIADO ---");
            _logger.LogInformation($"Para: {to}");
            _logger.LogInformation($"Asunto: {subject}");
            _logger.LogInformation($"Cuerpo: \n{body}");
            _logger.LogInformation("--------------------------");

            return Task.CompletedTask;
        }
    }
}
