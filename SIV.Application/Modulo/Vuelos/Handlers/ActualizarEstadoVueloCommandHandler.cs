using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Modulo.Vuelos.Events;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Vuelos.Handlers
{
    public class ActualizarEstadoVueloCommandHandler : IRequestHandler<ActualizarEstadoVueloCommand, Result<bool>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly ISeguridadService _seguridadService;
        private readonly IMediator _mediator;

        public ActualizarEstadoVueloCommandHandler(
            IVueloRepository vueloRepository,
            ISeguridadService seguridadService,
            IMediator mediator)
        {
            _vueloRepository = vueloRepository;
            _seguridadService = seguridadService;
            _mediator = mediator;
        }

        public async Task<Result<bool>> Handle(ActualizarEstadoVueloCommand request, CancellationToken cancellationToken)
        {
            var vuelo = await _vueloRepository.ObtenerPorIdAsync(request.VueloId);
            if (vuelo == null) return Result<bool>.Failure("El vuelo no existe.", StatusCodes.Status404NotFound);

            if (request.NuevoEstado == EstadoVuelo.Retrasado)
            {
                vuelo.ActualizarHorarioEstimado(DateTime.UtcNow.AddHours(1), request.MotivoCambio);
            }
            else
            {
                vuelo.CambiarEstado(request.NuevoEstado, request.MotivoCambio);
            }

            await _vueloRepository.ActualizarAsync(vuelo);

            var usuarioActual = _seguridadService.ObtenerUsarioActual();

            await _mediator.Publish(new VueloModificadoEvent
            {
                VueloId = vuelo.Id,
                NumeroVuelo = vuelo.NumeroVuelo,
                NuevoEstado = vuelo.EstadoActual.ToString(),
                MotivoCambio = $"Se cambió el estado del vuelo {vuelo.NumeroVuelo} a {vuelo.EstadoActual}. Motivo: {request.MotivoCambio}",
                Usuario = usuarioActual,
                Accion = "ActualizarEstado"
            }, cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}