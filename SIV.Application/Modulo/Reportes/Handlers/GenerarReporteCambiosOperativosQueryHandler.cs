using MediatR;
using SIV.Application.Modulo.Reportes.DTOs;
using SIV.Application.Modulo.Reportes.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Reportes.Handlers
{
    public class GenerarReporteCambiosOperativosQueryHandler : IRequestHandler<GenerarReporteCambiosOperativosQuery, Result<IEnumerable<ReporteCambioOperativoDto>>>
    {
        private readonly IReportesRepository _reportesRepository;

        public GenerarReporteCambiosOperativosQueryHandler(IReportesRepository reportesRepository)
        {
            _reportesRepository = reportesRepository;
        }

        public async Task<Result<IEnumerable<ReporteCambioOperativoDto>>> Handle(GenerarReporteCambiosOperativosQuery request, CancellationToken cancellationToken)
        {
            if (request.FechaInicio > request.FechaFin)
            {
                return Result<IEnumerable<ReporteCambioOperativoDto>>.Failure("La fecha de inicio no puede ser mayor que la fecha de fin.", 400);
            }

            var resultados = await _reportesRepository.ObtenerCambiosOperativosAsync(request.FechaInicio, request.FechaFin);

            var dtos = resultados.Select(r => new ReporteCambioOperativoDto
            {
                CambioId = r.Cambio.Id,
                VueloId = r.Cambio.VueloId,
                NumeroVuelo = r.NumeroVuelo,
                TipoCambio = r.Cambio.TipoCambio,
                Motivo = r.Cambio.Motivo,
                DetalleCambio = r.Cambio.DetalleCambio,
                FechaHora = r.Cambio.FechaHora,
                OperadorResponsable = r.Operador
            }).ToList();

            return Result<IEnumerable<ReporteCambioOperativoDto>>.Success(dtos);
        }
    }
}
