using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.Common.Models;
using SIV.Application.Modulo.Auditoria.DTOs;
using SIV.Application.Modulo.Auditoria.Queries;
using SIV.Domain.Common;

namespace SIV.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RolesConstantes.Administrador + "," + RolesConstantes.Auditor)]
    public class AuditoriaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuditoriaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("logs")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<LogAuditoriaDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(string))]
        public async Task<ActionResult<PaginatedList<LogAuditoriaDto>>> ObtenerLogs(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] DateTime? fechaInicio = null,
            [FromQuery] DateTime? fechaFin = null,
            [FromQuery] string? accion = null)
        {
            var query = new ConsultarLogAuditoriaQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Accion = accion
            };

            var result = await _mediator.Send(query);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("exportar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(string))]
        public async Task<IActionResult> ExportarCsv(
            [FromQuery] DateTime? fechaInicio = null,
            [FromQuery] DateTime? fechaFin = null,
            [FromQuery] string? accion = null)
        {
            var query = new ExportarAuditoriaCsvQuery
            {
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Accion = accion
            };

            var result = await _mediator.Send(query);

            if (result.IsSuccess)
                return File(result.Value!, "text/csv", $"auditoria_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv");

            return BadRequest(result);
        }
    }
}