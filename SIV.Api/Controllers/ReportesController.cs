using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.Modulo.Reportes.DTOs;
using SIV.Application.Modulo.Reportes.Queries;
using SIV.Domain.Common;

namespace SIV.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RolesConstantes.Administrador)]
    public class ReportesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("estados")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VueloEstadoReporteDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(string))]
        public async Task<ActionResult<IEnumerable<VueloEstadoReporteDto>>> ObtenerPorEstado(
            [FromQuery] DateTime? fechaInicio = null,
            [FromQuery] DateTime? fechaFin = null)
        {
            var query = new ObtenerReporteVuelosPorEstadoQuery
            {
                FechaInicio = fechaInicio,
                FechaFin = fechaFin
            };

            var result = await _mediator.Send(query);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("top-seguimientos")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VueloMasSeguidoReporteDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(string))]
        public async Task<ActionResult<IEnumerable<VueloMasSeguidoReporteDto>>> ObtenerTopSeguimientos(
            [FromQuery] int top = 10)
        {
            var query = new ObtenerReporteVuelosMasSeguidosQuery
            {
                Top = top
            };

            var result = await _mediator.Send(query);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.ErrorMessage);
        }
    }
}