using Microsoft.EntityFrameworkCore;
using SIV.Domain.Entities.Sistema;
using SIV.Domain.Interfaces;
using SIV.Infrastructure.Persistence;

namespace SIV.Infrastructure.Persistence.Repositories
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

        public async Task<(IEnumerable<LogAuditoria> Logs, int TotalCount)> ObtenerLogsPaginadosAsync(int pageNumber, int pageSize, DateTime? fechaInicio, DateTime? fechaFin, string? accion, string? busqueda = null)
        {
            var query = _context.LogAuditorias.AsNoTracking().AsQueryable();

            if (fechaInicio.HasValue)           
                query = query.Where(l => l.FechaRegistro >= fechaInicio.Value);
            
            if (fechaFin.HasValue)          
                query = query.Where(l => l.FechaRegistro <= fechaFin.Value);
            
            if (!string.IsNullOrWhiteSpace(accion))           
                query = query.Where(l => l.Accion.Contains(accion));
            
            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                var busquedaLower = busqueda.ToLower().Trim();
                query = query.Where(l => l.Usuario.ToLower().Contains(busquedaLower) 
                                      || l.Accion.ToLower().Contains(busquedaLower) 
                                      || l.Detalles.ToLower().Contains(busquedaLower));
            }
            
            var totalCount = await query.CountAsync();

            var logs = await query
                .OrderByDescending(l => l.FechaRegistro)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (logs, totalCount);
        }
    }
}