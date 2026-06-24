using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.Modulo.Vuelos.DTOs;
using SIV.Application.Modulo.Vuelos.Queries;
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

        [HttpGet("estadisticas")]
        public async Task<ActionResult<EstadisticasVuelosDto>> ObtenerEstadisticasVuelos()
        {
            var query = new ObtenerEstadisticasVuelosQuery();
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("{id}/historial")]
        // [Authorize(Roles = ...)] <--- Coméntalo temporalmente
        public IActionResult GetHistorial(int id)
        {
            // 1. ¿Logró leer el token?
            var autenticado = User.Identity?.IsAuthenticated ?? false;

            // 2. ¿Detecta el rol?
            var esAdmin = User.IsInRole("Administrador");

            // 3. ¿Qué claims cargó en memoria?
            var listaClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

            return Ok(new
            {
                EstaAutenticado = autenticado,
                EsAdministrador = esAdmin,
                ClaimsRecibidos = listaClaims
            });
        }
    }
}