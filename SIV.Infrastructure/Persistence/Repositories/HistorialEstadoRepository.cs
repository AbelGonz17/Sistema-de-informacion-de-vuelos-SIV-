using Microsoft.EntityFrameworkCore;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;

namespace SIV.Infrastructure.Persistence
{
    public class HistorialEstadoRepository : IHistorialEstadoRepository
    {
        private readonly ApplicationDbContext _context;

        public HistorialEstadoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HistorialEstado>> ObtenerPorVueloAsync(Guid vueloId)
        {
            return await _context.HistorialEstados
                .AsNoTracking()
                .Where(h => h.VueloId == vueloId)
                .OrderByDescending(h => h.FechaHora)
                .ToListAsync();
        }

        public async Task AgregarAsync(HistorialEstado historial)
        {
            await _context.HistorialEstados.AddAsync(historial);
        }
    }
}
