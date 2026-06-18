using MediatR;
using SIV.Domain.Common;
using System;

namespace SIV.Application.Modulo.Auditoria.Queries
{
    public class ExportarAuditoriaCsvQuery : IRequest<Result<byte[]>>
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? Accion { get; set; }
    }
}
