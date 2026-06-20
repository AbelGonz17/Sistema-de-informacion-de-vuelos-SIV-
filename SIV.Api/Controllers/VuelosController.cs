using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.Common.Mappings;
using SIV.Application.Common.Models;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Application.Modulo.Vuelos.Queries;
using SIV.Domain.Common;

namespace SIV.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VuelosController : ControllerBase
    {
        private readonly IMediator _mediator;
        public VuelosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("registrar")]
        [Authorize(Roles = RolesConstantes.Operador )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<Guid>> RegistrarVuelo([FromBody] CrearVueloCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result);
        }

        [HttpPost("actualizar-estado")]
        [Authorize(Roles = RolesConstantes.Operador)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<bool>> ActualizarEstado([FromBody] ActualizarEstadoVueloCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result);
        }

        [HttpPost("registrar-retraso")]
        [Authorize(Roles = RolesConstantes.Operador)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<bool>> RegistrarRetraso([FromBody] RegistrarRetrasoCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result);
        }

        [HttpPost("asignar-puerta")]
        [Authorize(Roles = RolesConstantes.Operador)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<ActionResult<bool>> AsignarPuerta([FromBody] AsignarPuertaCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("fids")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<VueloTableroDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<ActionResult<PaginatedList<VueloTableroDto>>> ObtenerTableroFids(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] bool? esLlegada = null,
            [FromQuery] string? estado = null,
            [FromQuery] Guid? aerolineaId = null,
            [FromQuery] DateTime? fecha = null)
        {
            var query = new ConsultarTableroFidsQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                EsLlegada = esLlegada,
                Estado = estado,
                AerolineaId = aerolineaId,
                Fecha = fecha
            };

            var result = await _mediator.Send(query);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("buscar/{numeroVuelo}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<object>> BuscarPorNumero(string numeroVuelo)
        {
            var query = new BuscarVueloEspecificoQuery { NumeroVuelo = numeroVuelo };
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result);
        }

        [HttpPost("registrar-adelanto")]
        [Authorize(Roles = RolesConstantes.Operador)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<ActionResult<bool>> RegistrarAdelanto([FromBody] RegistrarAdelantoCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("{id}/historial")]
        [Authorize(Roles = RolesConstantes.Administrador + "," + RolesConstantes.Operador + "," + RolesConstantes.Auditor)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerHistorial(Guid id)
        {
            var query = new ObtenerHistorialVueloQuery(id);
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
                return Ok(result.Value);

            return NotFound(result.ErrorMessage);
        }
    }
}