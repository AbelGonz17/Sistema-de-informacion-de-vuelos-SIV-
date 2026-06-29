using Microsoft.EntityFrameworkCore;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;

namespace SIV.Infrastructure.Persistence
{
    public class AeropuertoRepository : IAeropuertoRepository
    {
        private readonly ApplicationDbContext _context;

        public AeropuertoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Aeropuerto>> ObtenerTodosAsync()
        {
            return await _context.Aeropuertos.AsNoTracking().ToListAsync();
        }

        public async Task<Aeropuerto?> ObtenerPorIdAsync(Guid id)
        {
            return await _context.Aeropuertos.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> ExisteCodigoParaOtroAeropuertoAsync(Guid idActual, string codigo)
        {
            return await _context.Aeropuertos
                .AnyAsync(a => a.Id != idActual && a.Codigo.ToLower().Trim() == codigo.ToLower().Trim());
        }

        public async Task AgregarAsync(Aeropuerto aeropuerto)
        {
            await _context.Aeropuertos.AddAsync(aeropuerto);
        }

        public Task ActualizarAsync(Aeropuerto aeropuerto)
        {
            _context.Aeropuertos.Update(aeropuerto);
            return Task.CompletedTask;
        }

        public Task EliminarAsync(Aeropuerto aeropuerto)
        {
            aeropuerto.Desactivar();
            return Task.CompletedTask;
        }
    }
}