using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.Modulo.Vuelos.DTOs;
using SIV.Application.Modulo.Vuelos.Queries;
using SIV.Application.Modulo.Reportes.DTOs;
using SIV.Application.Modulo.Reportes.Queries;
using SIV.Domain.Common;
using System.Threading.Tasks;

namespace SIV.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = RolesConstantes.Administrador + "," + RolesConstantes.Auditor)]
    public class ReportesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("operacion")]
        [Authorize(Roles = RolesConstantes.Administrador + "," + RolesConstantes.Auditor)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReporteOperacionDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(string))]
        public async Task<ActionResult<ReporteOperacionDto>> GenerarReporteOperacion([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            var query = new GenerarReporteOperacionQuery(fechaInicio, fechaFin);
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("cambios-operativos")]
        [Authorize(Roles = RolesConstantes.Administrador + "," + RolesConstantes.Auditor)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReporteCambioOperativoDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(string))]
        public async Task<ActionResult<IEnumerable<ReporteCambioOperativoDto>>> GenerarReporteCambiosOperativos([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            var query = new GenerarReporteCambiosOperativosQuery(fechaInicio, fechaFin);
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("seguimiento")]
        [Authorize(Roles = RolesConstantes.Administrador + "," + RolesConstantes.Auditor)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReporteSeguimientoDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(string))]
        public async Task<ActionResult<ReporteSeguimientoDto>> GenerarReporteSeguimiento([FromQuery] int top = 10)
        {
            var query = new GenerarReporteSeguimientoQuery(top);
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("exportar/{tipo}")]
        [Authorize(Roles = RolesConstantes.Administrador + "," + RolesConstantes.Auditor)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(string))]
        public async Task<IActionResult> ExportarReporteCsv(string tipo, [FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
        {
            var query = new ExportarReportesCsvQuery
            {
                TipoReporte = tipo,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin
            };

            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return File(result.Value, "text/csv", $"Reporte_{tipo}_{DateTime.Now:yyyyMMdd}.csv");
            }

            return BadRequest(result.ErrorMessage);
        }
    }
}