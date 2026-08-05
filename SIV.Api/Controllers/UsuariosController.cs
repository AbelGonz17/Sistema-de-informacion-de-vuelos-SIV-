using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Application.Modulo.Usuarios.Queries;
using SIV.Domain.Common;
using SIV.Application.Common.Mappings;
using SIV.Application.Modulo.Usuarios.DTOs;

namespace SIV.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public UsuariosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] AutenticarUsuarioQuery query)
        {
            query.IpAddress = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "unknown";
            var result = await _mediator.Send(query);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken([FromBody] RefreshRequest request)
        {
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "unknown";
            var command = new RefrescarTokenCommand(request.AccessToken, request.RefreshToken, ipAddress);
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        public record RefreshRequest(string AccessToken, string RefreshToken);

        [HttpPost("cerrar-sesion")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        public async Task<ActionResult<bool>> CerrarSesion([FromBody] CerrarSesionRequest request)
        {
            if (UsuarioId == Guid.Empty) 
                return Unauthorized();

            var command = new CerrarSesionCommand(UsuarioId, request?.RefreshToken);
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        public record CerrarSesionRequest(string? RefreshToken);

        [HttpPost("crear-interno")]
        [Authorize(Roles = RolesConstantes.Administrador)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<string>> CrearUsuarioInterno([FromBody] CrearUsuarioInternoCommand command)
        {
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}/interno")]
        [Authorize(Roles = RolesConstantes.Administrador)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<ActionResult<string>> ActualizarUsuarioInterno(Guid id, [FromBody] ActualizarUsuarioInternoCommand command)
        {
            if (id != command.Id)
                return BadRequest("El ID de la ruta no coincide con el cuerpo de la petición.");

            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result);
        }

        [HttpPost("registrar")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<TokenResponseDto>> Registrar([FromBody] RegistrarCuentaCommand command)
        {
            command.IpAddress = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "unknown";
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id}/desactivar")]
        [Authorize(Roles = RolesConstantes.Administrador)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<bool>> DesactivarUsuario(Guid id)
        {
            var command = new DesactivarUsuarioCommand(id);
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id}/activar")]
        [Authorize(Roles = RolesConstantes.Administrador)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<bool>> ActivarUsuario(Guid id)
        {
            var command = new ActivarUsuarioCommand(id);
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("cambiar-contrasena")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<bool>> CambiarContrasena([FromBody] CambiarContrasenaRequest request)
        {
            if (UsuarioId == Guid.Empty) 
                return Unauthorized();

            var command = new CambiarContrasenaCommand(UsuarioId, request.ContrasenaActual, request.NuevaContrasena);
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        public record CambiarContrasenaRequest(string ContrasenaActual, string NuevaContrasena);

        [HttpPost("olvide-contrasena")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<bool>> OlvideContrasena([FromBody] OlvideContrasenaCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("restablecer-contrasena")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<bool>> RestablecerContrasena([FromBody] RestablecerContrasenaCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("mis-seguimientos")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<HistorialSeguimientoDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<IEnumerable<HistorialSeguimientoDto>>> ObtenerMisSeguimientos()
        {
            if (UsuarioId == Guid.Empty) 
                return Unauthorized();

            var query = new ConsultarVuelosEnSeguimientoQuery(UsuarioId);
            var result = await _mediator.Send(query);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("seguimientos/{vueloId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<bool>> IniciarSeguimiento(Guid vueloId)
        {
            if (UsuarioId == Guid.Empty)
                return Unauthorized(new { error = "Identificación de usuario inválida o ausente en el token." });

            var command = new IniciarSeguimientoCommand(
                UsuarioId,
                vueloId
            );

            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("seguimientos/{vueloId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<bool>> DejarDeSeguir(Guid vueloId)
        {
            if (UsuarioId == Guid.Empty)
                return Unauthorized(new { error = "Identificación de usuario inválida o ausente en el token." });

            var command = new DejarDeSeguirCommand(
                UsuarioId,
                vueloId
            );

            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("mis-notificaciones")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<NotificacionDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<IEnumerable<NotificacionDto>>> ObtenerMisNotificaciones()
        {
            if (UsuarioId == Guid.Empty)
                return Unauthorized();        

            var query = new ConsultarNotificacionesUsuarioQuery(UsuarioId);
            var result = await _mediator.Send(query);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
               
        }

        [HttpPut("notificaciones/{id}/marcar-leida")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<bool>> MarcarNotificacionLeida(Guid id)
        {
            if (UsuarioId == Guid.Empty)
                return Unauthorized();    

            var command = new MarcarNotificacionLeidaCommand(id, UsuarioId);
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("internos")]
        [Authorize(Roles = RolesConstantes.Administrador + "," + RolesConstantes.Auditor)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UsuarioInternoDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<UsuarioInternoDto>>> ObtenerUsuariosInternos()
        {
            var result = await _mediator.Send(new ObtenerUsuariosInternosQuery());
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("publicos")]
        [Authorize(Roles = RolesConstantes.Administrador + "," + RolesConstantes.Auditor)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UsuarioPublicoDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<UsuarioPublicoDto>>> ObtenerUsuariosPublicos()
        {
            var result = await _mediator.Send(new ObtenerUsuariosPublicosQuery());
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}