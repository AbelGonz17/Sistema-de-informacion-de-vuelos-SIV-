using MediatR;
using SIV.Domain.Common;
using System;

namespace SIV.Application.Modulo.Reportes.Queries
{
    public class ExportarReportesCsvQuery : IRequest<Result<byte[]>>
    {
        public string TipoReporte { get; set; } = string.Empty; // "Operacion", "Cambios", "Seguimientos"
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
