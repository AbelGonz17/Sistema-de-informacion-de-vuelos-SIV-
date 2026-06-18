using MediatR;
using SIV.Application.Modulo.Reportes.DTOs;
using SIV.Application.Modulo.Reportes.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Modulo.Reportes.Handlers
{
    public class ObtenerReporteVuelosPorEstadoQueryHandler : IRequestHandler<ObtenerReporteVuelosPorEstadoQuery, Result<IEnumerable<VueloEstadoReporteDto>>>
    {
        private readonly IReportesRepository _reportesRepository;

        public ObtenerReporteVuelosPorEstadoQueryHandler(IReportesRepository reportesRepository)
        {
            _reportesRepository = reportesRepository;
        }

        public async Task<Result<IEnumerable<VueloEstadoReporteDto>>> Handle(ObtenerReporteVuelosPorEstadoQuery request, CancellationToken cancellationToken)
        {
            var conteo = await _reportesRepository.ObtenerConteoVuelosPorEstadoAsync(request.FechaInicio, request.FechaFin);

            var resultado = conteo.Select(kvp => new VueloEstadoReporteDto
            {
                Estado = kvp.Key,
                Cantidad = kvp.Value
            }).ToList();

            return Result<IEnumerable<VueloEstadoReporteDto>>.Success(resultado);
        }
    }

    public class ObtenerReporteVuelosMasSeguidosQueryHandler : IRequestHandler<ObtenerReporteVuelosMasSeguidosQuery, Result<IEnumerable<VueloMasSeguidoReporteDto>>>
    {
        private readonly IReportesRepository _reportesRepository;

        public ObtenerReporteVuelosMasSeguidosQueryHandler(IReportesRepository reportesRepository)
        {
            _reportesRepository = reportesRepository;
        }

        public async Task<Result<IEnumerable<VueloMasSeguidoReporteDto>>> Handle(ObtenerReporteVuelosMasSeguidosQuery request, CancellationToken cancellationToken)
        {
            var vuelosTop = await _reportesRepository.ObtenerTopVuelosMasSeguidosAsync(request.Top);

            var resultado = vuelosTop.Select(v => new VueloMasSeguidoReporteDto
            {
                VueloId = v.VueloId,
                NumeroVuelo = v.NumeroVuelo,
                CantidadSeguidores = v.CantidadSeguidores
            }).ToList();

            return Result<IEnumerable<VueloMasSeguidoReporteDto>>.Success(resultado);
        }
    }
}
