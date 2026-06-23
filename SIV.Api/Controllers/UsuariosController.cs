using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Application.Modulo.Usuarios.Queries;
using SIV.Presentation.Common;
using SIV.Domain.Common;
using System.Security.Claims;

namespace SIV.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsuariosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AutenticarUsuarioQuery query)
        {
            var result = await _mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpPost("Crear-interno")]
        [Authorize(Roles = RolesConstantes.Administrador)]
        public async Task<IActionResult> CrearUsuarioInterno([FromBody] CrearUsuarioInternoCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("registrar")]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar([FromBody] RegistrarCuentaCommand command)
        {
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpDelete("{id}/desactivar")]
        [Authorize(Roles = RolesConstantes.Administrador)]
        public async Task<IActionResult> DesactivarUsuario(Guid id)
        {
            var command = new DesactivarUsuarioCommand(id);
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpGet("mis-seguimientos")]
        [Authorize]
        public async Task<IActionResult> ObtenerMisSeguimientos()
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim))
                return Unauthorized(new { error = "Identificación de usuario inválida o ausente en el token." });

            var query = new ConsultarVuelosEnSeguimientoQuery { UsuarioId = Guid.Parse(usuarioIdClaim) };
            var result = await _mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpPost("seguir")]
        [Authorize]
        public async Task<IActionResult> IniciarSeguimiento([FromQuery] Guid vueloId)
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim))
                return Unauthorized(new { error = "Identificación de usuario inválida o ausente en el token." });

            var command = new IniciarSeguimientoCommand(
                Guid.Parse(usuarioIdClaim),
                vueloId
            );

            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpDelete("dejar-de-seguir")]
        [Authorize]
        public async Task<IActionResult> DejarDeSeguir([FromQuery] Guid vueloId)
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim))
                return Unauthorized(new { error = "Identificación de usuario inválida o ausente en el token." });

            var command = new DejarDeSeguirCommand(
                Guid.Parse(usuarioIdClaim),
                vueloId
            );

            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpGet("mis-notificaciones")]
        [Authorize]
        public async Task<IActionResult> ObtenerMisNotificaciones()
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim))
                return Unauthorized(new { error = "Identificación de usuario inválida o ausente en el token." });

            var query = new ConsultarNotificacionesUsuarioQuery(Guid.Parse(usuarioIdClaim));
            var result = await _mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpPut("notificaciones/{id}/marcar-leida")]
        [Authorize]
        public async Task<IActionResult> MarcarNotificacionLeida(Guid id)
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim))
                return Unauthorized(new { error = "Identificación de usuario inválida o ausente en el token." });

            var command = new MarcarNotificacionLeidaCommand(id);
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }
    }
}