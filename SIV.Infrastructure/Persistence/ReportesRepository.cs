using Microsoft.EntityFrameworkCore;
using SIV.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIV.Infrastructure.Persistence
{
    public class ReportesRepository : IReportesRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, int>> ObtenerConteoVuelosPorEstadoAsync(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var query = _context.Vuelos.AsNoTracking().AsQueryable();

            if (fechaInicio.HasValue)
            {
                query = query.Where(v => v.HorarioPlanificadoSalida >= fechaInicio.Value);
            }

            if (fechaFin.HasValue)
            {
                query = query.Where(v => v.HorarioPlanificadoSalida <= fechaFin.Value);
            }

            var group = await query
                .GroupBy(v => v.EstadoActual)
                .Select(g => new { Estado = g.Key, Conteo = g.Count() })
                .ToListAsync();

            return group.ToDictionary(g => g.Estado.ToString(), g => g.Conteo);
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
                .Where(v => vueloIds.Contains(v.Id))
                .ToDictionaryAsync(v => v.Id, v => v.NumeroVuelo);

            return topSeguimientos.Select(x => (x.VueloId, vuelosDic.GetValueOrDefault(x.VueloId, "N/A"), x.Cantidad));
        }
    }
}
