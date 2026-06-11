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
            var query = _context.Vuelos.AsNoTracking();

            if (esLlegada)
            {
                query = query.Where(v => v.HorarioPlanificadoLlegada.Date == fecha.Date && v.Destino == "SDQ");
            }
            else
            {
                query = query.Where(v => v.HorarioPlanificadoSalida.Date == fecha.Date && v.Origen == "SDQ");
            }

            return await query.ToListAsync();
        }
        public async Task AgregarAsync(Vuelo vuelo)
        {
            await _context.Vuelos.AddAsync(vuelo);
        }

        public async Task ActualizarAsync(Vuelo vuelo)
        {
            _context.Vuelos.Update(vuelo);
        }

        public async Task<bool> ExisteVueloAsync(string numeroVuelo, string aerolinea, DateTime fecha, string origen, string destino)
        {
            return await _context.Vuelos
                .AnyAsync(v => v.NumeroVuelo == numeroVuelo
                            && v.Aerolinea == aerolinea
                            && v.HorarioPlanificadoSalida.Date == fecha.Date
                            && v.Origen == origen
                            && v.Destino == destino);
        }
    }
}