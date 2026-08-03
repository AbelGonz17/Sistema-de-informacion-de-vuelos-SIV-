using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.Modulo.Aerolineas.Commands;
using SIV.Application.Modulo.Aerolineas.Queries;
using SIV.Domain.Common;

namespace SIV.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AerolineasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AerolineasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = RolesConstantes.Administrador + "," + RolesConstantes.Operador + "," + RolesConstantes.Auditor)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(string))]
        public async Task<IActionResult> ObtenerTodas()
        {
            var result = await _mediator.Send(new ObtenerAerolineasQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = RolesConstantes.Administrador + "," + RolesConstantes.Operador + "," + RolesConstantes.Auditor)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerPorId(Guid id)
        {
            var result = await _mediator.Send(new ObtenerAerolineaPorIdQuery(id));

            if (result != null)
                return Ok(result);

            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = RolesConstantes.Administrador)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(string))]
        public async Task<IActionResult> Crear([FromBody] CrearAerolineaCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = RolesConstantes.Administrador)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(string))]
        public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarAerolineaCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = RolesConstantes.Administrador)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(string))]
        public async Task<IActionResult> Eliminar(Guid id)
        {
            var result = await _mediator.Send(new EliminarAerolineaCommand(id));

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.ErrorMessage);
        }
    }
}