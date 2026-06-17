using SIV.Domain.Entities;
using SIV.Domain.Interfaces;

namespace SIV.Infrastructure.Persistence
{
    public class AuditoriaRepository : IAuditoriaRepository
    {
        private readonly ApplicationDbContext _context;

        public AuditoriaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarLogAsync(LogAuditoria log)
        {
            await _context.LogAuditorias.AddAsync(log);
        }
    }
}