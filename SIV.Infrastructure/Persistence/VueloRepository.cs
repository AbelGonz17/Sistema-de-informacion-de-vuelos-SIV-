using Microsoft.EntityFrameworkCore;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;

namespace SIV.Infrastructure.Persistence
{
    public class VueloRepository : IVueloRepository
    {
        private readonly ApplicationDbContext _context;

        public VueloRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Vuelo> ObtenerPorIdAsync(Guid Id)
        {
            return await _context.Vuelos.FindAsync(Id);
        }

        public async Task<Vuelo> ObtenerPorNumeroAsync(string numeroVuelo)
        {
            return await _context.Vuelos
                .FirstOrDefaultAsync(v => v.NumeroVuelo == numeroVuelo);
        }

        public async Task<IEnumerable<Vuelo>> ObtenerVuelosPorFechaYTipoAsync(DateTime fecha, bool esLlegada)
        {
            return await _context.Vuelos
                .AsNoTracking()
                .Where(v => v.HorarioPlanificadoSalida.Date == fecha.Date)
                .ToListAsync();
        }
        public async Task AgregarAsync (Vuelo vuelo)
        {
            await _context.Vuelos.AddAsync(vuelo);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Vuelo vuelo)
        {
            _context.Vuelos.Update(vuelo);
            await _context.SaveChangesAsync(); 
        }
    }
}