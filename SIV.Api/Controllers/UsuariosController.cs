using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Application.Modulo.Usuarios.Queries;
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
            var token = await _mediator.Send(query);
            return Ok(new { tokenAccess = token, tipo = "Bearer" });
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistrarCuentaCommand command)
        {
            var token = await _mediator.Send(command);
            return Ok(new { tokenAccess = token, tipo = "Bearer", mensaje = "Usuario registrado y autenticado con éxito." });
        }

        [HttpGet("mis-seguimientos")]
        [Authorize] 
        public async Task<IActionResult> ObtenerMisSeguimientos()
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim))
                return Unauthorized("Identificación de usuario inválida o ausente en el token.");

            var query = new ConsultarVuelosEnSeguimientoQuery { UsuarioId = Guid.Parse(usuarioIdClaim) };
            var resultado = await _mediator.Send(query);

            return Ok(resultado);
        }

        [HttpPost("seguir")]
        [Authorize] 
        public async Task<IActionResult> IniciarSeguimiento([FromQuery] Guid vueloId)
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim))
                return Unauthorized("Identificación de usuario inválida o ausente en el token.");

            var command = new IniciarSeguimientoCommand
            {
                UsuarioId = Guid.Parse(usuarioIdClaim),
                VueloId = vueloId
            };

            var resultado = await _mediator.Send(command);

            if (!resultado)
                return BadRequest("No se pudo procesar la solicitud de seguimiento.");

            return Ok(new { mensaje = "Te has suscrito exitosamente a las alertas en tiempo real de este vuelo." });
        }


        [HttpDelete("dejar-de-seguir")] 
        [Authorize] 
        public async Task<IActionResult> DejarDeSeguir([FromQuery] Guid vueloId)
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim))
                return Unauthorized("Identificación de usuario inválida o ausente en el token.");

            var command = new DejarDeSeguirCommand
            {
                UsuarioId = Guid.Parse(usuarioIdClaim),
                VueloId = vueloId
            };

            var resultado = await _mediator.Send(command);

            if (!resultado)
                return BadRequest("No se pudo procesar la solicitud de baja de seguimiento.");

            return Ok(new { mensaje = "Te has dado de baja. Ya no recibirás notificaciones sobre este vuelo." });
        }
    }
}