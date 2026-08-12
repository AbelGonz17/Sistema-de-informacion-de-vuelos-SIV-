using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.Common.Mappings;
using SIV.Application.Common.Models;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Application.Modulo.Vuelos.DTOs;
using SIV.Application.Modulo.Vuelos.Queries;
using SIV.Domain.Common;

namespace SIV.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VuelosController : ApiControllerBase
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
                return Ok(result);

            return BadRequest(result);
        }
        [HttpPost("upload")]
        [Authorize(Roles = RolesConstantes.Operador)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("multipart/form-data")]   
        public async Task<IActionResult> CargarVuelosMasivo([FromForm] CargarVuelosMasivoRequest request)
        {
            var command = new CargarVuelosMasivoCommand { Archivo = request.File };
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPut("{id}/basico")]
        [Authorize(Roles = RolesConstantes.Operador)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<ActionResult<Guid>> ActualizarBasico(Guid id, [FromBody] ActualizarDatosBasicosRequest request)
        {
            var command = new ActualizarDatosBasicosVueloCommand(
                id, request.Aerolinea, request.Origen, request.Destino,
                request.HorarioPlanificadoSalida, request.HorarioPlanificadoLlegada,
                request.Puerta, UsuarioId
            );
            
            var result = await _mediator.Send(command);

            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        public record ActualizarDatosBasicosRequest(Guid Aerolinea, Guid Origen, Guid Destino, DateTime HorarioPlanificadoSalida, DateTime HorarioPlanificadoLlegada, string Puerta);

        [HttpGet("{id}/detalle")]
        [Authorize(Roles = RolesConstantes.Administrador + "," + RolesConstantes.Operador + "," + RolesConstantes.Auditor + "," + RolesConstantes.Visitante)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerDetalle(Guid id)
        {
            var query = new ObtenerDetalleVueloQuery(id);
            var result = await _mediator.Send(query);

            if (result.IsSuccess) return Ok(result);
            return NotFound(result);
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
                return Ok(result);

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
                return Ok(result);

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
                return Ok(result);

            return BadRequest(result);
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
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("buscar/{numeroVuelo}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VueloDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<object>> BuscarPorNumero(string numeroVuelo)
        {
            var query = new BuscarVueloEspecificoQuery { NumeroVuelo = numeroVuelo };
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
                return Ok(result);

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
                return Ok(result);

            return BadRequest(result);
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
                return Ok(result);

            return NotFound(result);
        }

        [HttpPost("cancelar")]
        [Authorize(Roles = RolesConstantes.Operador)]
        public async Task<ActionResult<bool>> CancelarVuelo([FromBody] CancelarVueloCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }
    }
}