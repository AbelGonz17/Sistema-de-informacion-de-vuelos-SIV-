using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIV.Application.Modulo.Aeropuertos.Commands;
using SIV.Application.Modulo.Aeropuertos.Queries;
using SIV.Domain.Common;

namespace SIV.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AeropuertosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AeropuertosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = RolesConstantes.Administrador + "," + RolesConstantes.Operador + "," + RolesConstantes.Auditor)]
        public async Task<IActionResult> ObtenerTodos()
        {
            var result = await _mediator.Send(new ObtenerAeropuertosQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = RolesConstantes.Administrador + "," + RolesConstantes.Operador + "," + RolesConstantes.Auditor)]
        public async Task<IActionResult> ObtenerPorId(Guid id)
        {
            var result = await _mediator.Send(new ObtenerAeropuertoPorIdQuery(id)); 

            if (result != null)
                return Ok(result);

            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = RolesConstantes.Administrador)]
        public async Task<IActionResult> Crear([FromBody] CrearAeropuertoCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = RolesConstantes.Administrador)]
        public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarAeropuertoCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = RolesConstantes.Administrador)]
        public async Task<IActionResult> Eliminar(Guid id)
        {
            var result = await _mediator.Send(new EliminarAeropuertoCommand(id));

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }
    }
}