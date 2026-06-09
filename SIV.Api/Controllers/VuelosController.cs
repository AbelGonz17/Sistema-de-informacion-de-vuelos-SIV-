using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.Common.Mappings;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Application.Modulo.Vuelos.Queries;
using SIV.Presentation.Common;

namespace SIV.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VuelosController : ControllerBase
    {
        private readonly IMediator _mediator;
        public VuelosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarVuelo([FromBody] CrearVueloCommand command)
        {
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpPost("actualizar-estado")]
        [Authorize]
        public async Task<IActionResult> ActualizarEstado([FromBody] ActualizarEstadoVueloCommand command)
        {
            var result = await _mediator.Send(command);
    
            return result.ToActionResult();
        }

        [HttpPost("registrar-retraso")]
        public async Task<IActionResult> RegistrarRetraso([FromBody] RegistrarRetrasoCommand command)
        {
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        [HttpGet("tablero")]
        public async Task<IActionResult> ObtenerTablero([FromQuery] DateTime fecha, [FromQuery] bool esLlegada)
        {
            var query = new ConsultarTableroLlegadasQuery { Fecha = fecha, EsLlegada = esLlegada };
            var result = await _mediator.Send(query);

            return result.ToActionResult();
        }

        [HttpGet("buscar/{numeroVuelo}")]
        public async Task<IActionResult> BuscarPorNumero(string numeroVuelo)
        {
            var query = new BuscarVueloEspecificoQuery { NumeroVuelo = numeroVuelo };
            var result = await _mediator.Send(query);

            return result.ToActionResult();
        }
    }
}

