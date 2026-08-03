using Microsoft.EntityFrameworkCore;
using SIV.Domain.Entities.Catalogo;
using SIV.Domain.Interfaces;
using SIV.Infrastructure.Persistence;

namespace SIV.Infrastructure.Persistence.Repositories
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
            return await _context.Aerolineas.IgnoreQueryFilters().AsNoTracking().ToListAsync();
        }

        public async Task<Aerolinea?> ObtenerPorCodigoAsync(string codigo)
        {
            return await _context.Aerolineas.FirstOrDefaultAsync(a => a.Codigo == codigo);
        }

        public async Task<Aerolinea?> ObtenerPorIdAsync(Guid id)
        {
            return await _context.Aerolineas.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AgregarAsync(Aerolinea aerolinea)
        {
            await _context.Aerolineas.AddAsync(aerolinea);
        }

        public async Task<bool> ExisteCodigoParaOtraAerolineaAsync(Guid idActual, string codigo)
        {
            return await _context.Aerolineas
                .AnyAsync(a => a.Id != idActual && a.Codigo.ToLower().Trim() == codigo.ToLower().Trim());
        }

        public Task ActualizarAsync(Aerolinea aerolinea)
        {
            _context.Aerolineas.Update(aerolinea);
            return Task.CompletedTask;
        }

        public Task EliminarAsync(Aerolinea aerolinea)
        {
            aerolinea.Desactivar();
            return Task.CompletedTask;
        }
    }
}