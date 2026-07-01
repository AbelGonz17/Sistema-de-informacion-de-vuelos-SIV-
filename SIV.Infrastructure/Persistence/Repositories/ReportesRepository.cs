using Microsoft.EntityFrameworkCore;
using SIV.Domain.Entities.Vuelos;
using SIV.Domain.Interfaces;
using SIV.Infrastructure.Persistence;

namespace SIV.Infrastructure.Persistence.Repositories
{
    public class ReportesRepository : IReportesRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vuelo>> ObtenerVuelosPorRangoFechaAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Vuelos
                .IgnoreQueryFilters()
                .Include(v => v.AerolineaRef)
                .Include(v => v.OrigenRef)
                .Include(v => v.DestinoRef)
                .AsNoTracking()
                .Where(v => v.HorarioPlanificadoSalida >= fechaInicio && v.HorarioPlanificadoSalida <= fechaFin)
                .OrderBy(v => v.HorarioPlanificadoSalida)
                .ToListAsync();
        }

        public async Task<IEnumerable<(HistorialCambioOperativo Cambio, string NumeroVuelo, string Operador)>> ObtenerCambiosOperativosAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            var query = from cambio in _context.HistorialCambiosOperativos
                        join vuelo in _context.Vuelos.IgnoreQueryFilters() on cambio.VueloId equals vuelo.Id
                        join usuario in _context.Usuarios.IgnoreQueryFilters() on cambio.UsuarioResponsable equals usuario.Id
                        where cambio.FechaHora >= fechaInicio && cambio.FechaHora <= fechaFin
                        orderby cambio.FechaHora descending
                        select new
                        {
                            Cambio = cambio,
                            NumeroVuelo = vuelo.NumeroVuelo,
                            Operador = usuario.Nombre
                        };

            var resultados = await query.AsNoTracking().ToListAsync();

            return resultados.Select(x => (x.Cambio, x.NumeroVuelo, x.Operador));
        }

        public async Task<IEnumerable<(Guid VueloId, string NumeroVuelo, int CantidadSeguidores)>> ObtenerTopVuelosMasSeguidosAsync(int top)
        {
            var topSeguimientos = await _context.Seguimientos
                .Where(s => s.Activo)
                .GroupBy(s => s.VueloId)
                .Select(g => new { VueloId = g.Key, Cantidad = g.Count() })
                .OrderByDescending(x => x.Cantidad)
                .Take(top)
                .ToListAsync();

            var vueloIds = topSeguimientos.Select(x => x.VueloId).ToList();

            var vuelosDic = await _context.Vuelos
                .IgnoreQueryFilters()
                .Where(v => vueloIds.Contains(v.Id))
                .ToDictionaryAsync(v => v.Id, v => v.NumeroVuelo);

            return topSeguimientos.Select(x => (x.VueloId, vuelosDic.GetValueOrDefault(x.VueloId, "N/A"), x.Cantidad));
        }

        public async Task<int> ObtenerTotalUsuariosConSeguimientosActivosAsync()
        {
            return await _context.Seguimientos
                .Where(s => s.Activo)
                .Select(s => s.UsuarioId)
                .Distinct()
                .CountAsync();
        }
    }
}