using MediatR;
using SIV.Application.Common.Models;
using SIV.Application.Modulo.Auditoria.DTOs;
using SIV.Application.Modulo.Auditoria.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Modulo.Auditoria.Handlers
{
    public class ConsultarLogAuditoriaQueryHandler : IRequestHandler<ConsultarLogAuditoriaQuery, Result<PaginatedList<LogAuditoriaDto>>>
    {
        private readonly IAuditoriaRepository _auditoriaRepository;

        public ConsultarLogAuditoriaQueryHandler(IAuditoriaRepository auditoriaRepository)
        {
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task<Result<PaginatedList<LogAuditoriaDto>>> Handle(ConsultarLogAuditoriaQuery request, CancellationToken cancellationToken)
        {
            var (logs, totalCount) = await _auditoriaRepository.ObtenerLogsPaginadosAsync(
                request.PageNumber,
                request.PageSize,
                request.FechaInicio,
                request.FechaFin,
                request.Accion
            );

            var logsDto = logs.Select(l => new LogAuditoriaDto
            {
                Id = l.Id,
                FechaHora = l.FechaRegistro,
                Usuario = l.Usuario ?? "Sistema",
                Accion = l.Accion,
                Detalles = l.Detalles,
                EntidadAfectada = "",
                EntidadId = ""
            }).ToList();

            var paginatedResult = new PaginatedList<LogAuditoriaDto>(logsDto, totalCount, request.PageNumber, request.PageSize);

            return Result<PaginatedList<LogAuditoriaDto>>.Success(paginatedResult);
        }
    }
}
