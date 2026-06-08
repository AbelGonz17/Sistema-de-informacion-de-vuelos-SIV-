using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.Modulo.Usuarios.Commands;
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
    }
}