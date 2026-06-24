using MediatR;
using SIV.Application.Modulo.Reportes.DTOs;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Reportes.Queries
{
    public class GenerarReporteCambiosOperativosQuery : IRequest<Result<IEnumerable<ReporteCambioOperativoDto>>>
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public GenerarReporteCambiosOperativosQuery(DateTime fechaInicio, DateTime fechaFin)
        {
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
        }
    }
}
