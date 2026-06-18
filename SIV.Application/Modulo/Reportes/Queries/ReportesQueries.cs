using MediatR;
using SIV.Application.Modulo.Reportes.DTOs;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Reportes.Queries
{
    public class ObtenerReporteVuelosPorEstadoQuery : IRequest<Result<IEnumerable<VueloEstadoReporteDto>>>
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }

    public class ObtenerReporteVuelosMasSeguidosQuery : IRequest<Result<IEnumerable<VueloMasSeguidoReporteDto>>>
    {
        public int Top { get; set; } = 10;
    }
}