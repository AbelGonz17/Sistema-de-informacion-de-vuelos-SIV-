using MediatR;
using SIV.Application.Modulo.Reportes.DTOs;
using SIV.Application.Modulo.Reportes.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Reportes.Handlers
{
    public class GenerarReporteSeguimientoQueryHandler : IRequestHandler<GenerarReporteSeguimientoQuery, Result<ReporteSeguimientoDto>>
    {
        private readonly IReportesRepository _reportesRepository;

        public GenerarReporteSeguimientoQueryHandler(IReportesRepository reportesRepository)
        {
            _reportesRepository = reportesRepository;
        }

        public async Task<Result<ReporteSeguimientoDto>> Handle(GenerarReporteSeguimientoQuery request, CancellationToken cancellationToken)
        {
            var topVuelos = await _reportesRepository.ObtenerTopVuelosMasSeguidosAsync(request.Top);
            var totalUsuarios = await _reportesRepository.ObtenerTotalUsuariosConSeguimientosActivosAsync();

            var reporte = new ReporteSeguimientoDto
            {
                TotalUsuariosConSeguimientosActivos = totalUsuarios,
                TopVuelosMasSeguidos = topVuelos.Select(t => new VueloMasSeguidoReporteDto
                {
                    VueloId = t.VueloId,
                    NumeroVuelo = t.NumeroVuelo,
                    CantidadSeguidores = t.CantidadSeguidores
                }).ToList()
            };

            return Result<ReporteSeguimientoDto>.Success(reporte);
        }
    }
}
