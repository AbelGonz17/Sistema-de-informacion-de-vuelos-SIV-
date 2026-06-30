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

        public async Task<Vuelo> ObtenerPorIdConHistorialAsync(Guid id)
        {
            return await _context.Vuelos
                .Include(v => v.HistorialEstados)
                .Include(v => v.HistorialCambio)
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Vuelo> ObtenerDetalleCompletoAsync(Guid id)
        {
            return await _context.Vuelos
                .Include(v => v.AerolineaRef)
                .Include(v => v.OrigenRef)
                .Include(v => v.DestinoRef)
                .Include(v => v.HistorialEstados)
                .Include(v => v.HistorialCambio)
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Vuelo> ObtenerPorNumeroAsync(string numeroVuelo)
        {
            return await _context.Vuelos
                .FirstOrDefaultAsync(v => v.NumeroVuelo == numeroVuelo);
        }

        public async Task<bool> ExistenVuelosActivosPorAerolineaAsync(Guid aerolineaId)
        {
            return await _context.Vuelos
                .AnyAsync(v => v.Aerolinea == aerolineaId && v.Activo
                            && v.EstadoActual != SIV.Domain.Common.EstadoVuelo.Cancelado
                            && v.EstadoActual != SIV.Domain.Common.EstadoVuelo.Completado);
        }

        public async Task<IEnumerable<Vuelo>> ObtenerVuelosPorFechaYTipoAsync(DateTime fecha, bool esLlegada)
        {
            var query = _context.Vuelos
                .Include(v => v.AerolineaRef)
                .Include(v => v.OrigenRef)
                .Include(v => v.DestinoRef)
                .AsNoTracking(); 

            if (esLlegada)
            {
                query = query.Where(v => v.HorarioPlanificadoLlegada.Date == fecha.Date
                                      && v.DestinoRef.Pais == "SDQ"); 
            }
            else
            {
                query = query.Where(v => v.HorarioPlanificadoSalida.Date == fecha.Date
                                      && v.OrigenRef.Pais == "SDQ"); 
            }

            return await query.ToListAsync();
        }
        public async Task AgregarAsync(Vuelo vuelo)
        {
            await _context.Vuelos.AddAsync(vuelo);
        }

        public Task ActualizarAsync(Vuelo vuelo)
        {
            // EF Core ya está trackeando la entidad porque se obtuvo con FindAsync,
            // llamar a Update() marca incorrectamente como "Modified" a las entidades recién agregadas (como HistorialEstado).
            return Task.CompletedTask;
        }

        public async Task<bool> ExisteVueloAsync(string numeroVuelo, Guid aerolinea, DateTime fecha, Guid origen, Guid destino)
        {
            return await _context.Vuelos
                .AnyAsync(v => v.NumeroVuelo == numeroVuelo
                            && v.Aerolinea == aerolinea
                            && v.HorarioPlanificadoSalida.Date == fecha.Date
                            && v.Origen == origen
                            && v.Destino == destino);
        }

        public async Task<(IEnumerable<Vuelo> Vuelos, int TotalCount)> ObtenerVuelosFidsPaginadosAsync(int pageNumber, int pageSize, bool? esLlegada, string? estado, Guid? aerolineaId, DateTime? fecha)
        {
            var query = _context.Vuelos
                .Include(v => v.AerolineaRef)
                .Include(v => v.OrigenRef)
                .Include(v => v.DestinoRef)
                .AsNoTracking()
                .AsQueryable();

            if (fecha.HasValue)
            {
                if (esLlegada.HasValue && esLlegada.Value)
                {
                    query = query.Where(v => v.HorarioPlanificadoLlegada.Date == fecha.Value.Date);
                }
                else if (esLlegada.HasValue && !esLlegada.Value)
                {
                    query = query.Where(v => v.HorarioPlanificadoSalida.Date == fecha.Value.Date);
                }
                else
                {
                    query = query.Where(v => v.HorarioPlanificadoLlegada.Date == fecha.Value.Date || v.HorarioPlanificadoSalida.Date == fecha.Value.Date);
                }
            }

            if (!string.IsNullOrWhiteSpace(estado) && Enum.TryParse<SIV.Domain.Common.EstadoVuelo>(estado, out var estadoEnum))
            {
                query = query.Where(v => v.EstadoActual == estadoEnum);
            }

            if (aerolineaId.HasValue && aerolineaId.Value != Guid.Empty)
            {
                query = query.Where(v => v.Aerolinea == aerolineaId.Value);
            }

            if (esLlegada.HasValue)
            {
                if (esLlegada.Value)
                {
                    query = query.OrderBy(v => v.HorarioPlanificadoLlegada);
                }
                else
                {
                    query = query.OrderBy(v => v.HorarioPlanificadoSalida);
                }
            }
            else
            {
                query = query.OrderBy(v => v.HorarioPlanificadoSalida);
            }

            var totalCount = await query.CountAsync();

            var vuelos = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (vuelos, totalCount);
        }

        public async Task<IEnumerable<Vuelo>> ObtenerTodosAsync()
        {
            return await _context.Vuelos.AsNoTracking().ToListAsync();
        }
    }
}