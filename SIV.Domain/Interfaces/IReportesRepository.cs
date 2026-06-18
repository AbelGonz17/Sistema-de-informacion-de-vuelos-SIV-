using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIV.Domain.Interfaces
{
    public interface IReportesRepository
    {
        Task<Dictionary<string, int>> ObtenerConteoVuelosPorEstadoAsync(DateTime? fechaInicio, DateTime? fechaFin);
        Task<IEnumerable<(Guid VueloId, string NumeroVuelo, int CantidadSeguidores)>> ObtenerTopVuelosMasSeguidosAsync(int top);
    }
}
