using Microsoft.EntityFrameworkCore;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;

namespace SIV.Infrastructure.Persistence
{
    public class HistorialCambioOperativoRepository : IHistorialCambioOperativoRepository
    {
        private readonly ApplicationDbContext _context;

        public HistorialCambioOperativoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HistorialCambioOperativo>> ObtenerPorVueloAsync(Guid vueloId)
        {
            return await _context.HistorialCambiosOperativos
                .AsNoTracking()
                .Where(h => h.VueloId == vueloId)
                .OrderByDescending(h => h.FechaHora)
                .ToListAsync();
        }

        public async Task AgregarAsync(HistorialCambioOperativo historial)
        {
            await _context.HistorialCambiosOperativos.AddAsync(historial);
        }
    }
}
