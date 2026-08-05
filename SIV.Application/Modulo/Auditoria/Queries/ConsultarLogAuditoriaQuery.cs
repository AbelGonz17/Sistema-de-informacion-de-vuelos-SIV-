using MediatR;
using SIV.Application.Common.Models;
using SIV.Application.Modulo.Auditoria.DTOs;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Auditoria.Queries
{
    public class ConsultarLogAuditoriaQuery : IRequest<Result<PaginatedList<LogAuditoriaDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? Accion { get; set; }
        public string? Busqueda { get; set; }
    }
}