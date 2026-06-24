using MediatR;
using SIV.Application.Modulo.Reportes.DTOs;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Reportes.Queries
{
    public class GenerarReporteOperacionQuery : IRequest<Result<ReporteOperacionDto>>
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        
        public GenerarReporteOperacionQuery(DateTime fechaInicio, DateTime fechaFin)
        {
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
        }
    }
}
