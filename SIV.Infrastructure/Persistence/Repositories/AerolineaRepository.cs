using Microsoft.EntityFrameworkCore;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;

namespace SIV.Infrastructure.Persistence
{
    public class AerolineaRepository : IAerolineaRepository
    {
        private readonly ApplicationDbContext _context;

        public AerolineaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Aerolinea>> ObtenerTodasAsync()
        {
            return await _context.Aerolineas.AsNoTracking().ToListAsync();
        }

        public async Task<Aerolinea?> ObtenerPorIdAsync(Guid id)
        {
            return await _context.Aerolineas.FindAsync(id);
        }

        public async Task<Aerolinea?> ObtenerPorCodigoAsync(string codigo)
        {
            return await _context.Aerolineas.FirstOrDefaultAsync(a => a.Codigo == codigo);
        }

        public async Task AgregarAsync(Aerolinea aerolinea)
        {
            await _context.Aerolineas.AddAsync(aerolinea);
        }

        public async Task ActualizarAsync(Aerolinea aerolinea)
        {
            _context.Aerolineas.Update(aerolinea);
        }

        public async Task EliminarAsync(Aerolinea aerolinea)
        {
            _context.Aerolineas.Remove(aerolinea);
        }
    }
}
