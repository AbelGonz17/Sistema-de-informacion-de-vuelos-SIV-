using MediatR;
using SIV.Application.Modulo.Reportes.DTOs;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Reportes.Queries
{
    public class GenerarReporteSeguimientoQuery : IRequest<Result<ReporteSeguimientoDto>>
    {
        public int Top { get; set; } = 10;
        
        public GenerarReporteSeguimientoQuery(int top)
        {
            Top = top;
        }
    }
}
