using MediatR;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.Common.Mappings;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Application.Modulo.Vuelos.Queries;

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

        [HttpPost("actualizar-estado")]
        public async Task<IActionResult> ActualizarEstado([FromBody] ActualizarEstadoVueloCommand command)
        {
            var resultado = await _mediator.Send(command);

            if (!resultado) return BadRequest("No se pudo procesar la actualización del vuelo.");

            return Ok(new { mensaje = "Estado de vuelo actualizado exitosamente." });
        }

        [HttpPost("registrar-retraso")]
        public async Task<IActionResult> RegistrarRetraso([FromBody] RegistrarRetrasoCommand command)
        {
            var resultado = await _mediator.Send(command);

            if (!resultado) return BadRequest("No se pudo procesar el registro del retraso.");

            return Ok(new { mensaje = "Retraso operativo registrado y publicado con éxito." });
        }

        [HttpGet("tablero")]
        public async Task<ActionResult<IEnumerable<VueloDto>>> ObtenerTablero([FromQuery] DateTime fecha, [FromQuery] bool esLlegada)
        {
            var query = new ConsultarTableroLlegadasQuery { Fecha = fecha, EsLlegada = esLlegada };
            var resultado = await _mediator.Send(query);
            return Ok(resultado);
        }

        [HttpGet("buscar/{numeroVuelo}")]
        public async Task<ActionResult<VueloDto>> BuscarPorNumero(string numeroVuelo)
        {
            var query = new BuscarVueloEspecificoQuery { NumeroVuelo = numeroVuelo };
            var resultado = await _mediator.Send(query);

            if (resultado == null) return NotFound($"El vuelo número {numeroVuelo} no fue localizado.");

            return Ok(resultado);
        }
    }
}

