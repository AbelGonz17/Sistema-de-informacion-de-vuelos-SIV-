using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Vuelos.Handlers
{
    public class AsignarPuertaCommandHandler : IRequestHandler<AsignarPuertaCommand, Result<bool>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly ISeguridadService _seguridadService;
        private readonly IMediator _mediator;

        public AsignarPuertaCommandHandler(
            IVueloRepository vueloRepository,
            ISeguridadService seguridadService,
            IMediator mediator)
        {
            _vueloRepository = vueloRepository;
            _seguridadService = seguridadService;
            _mediator = mediator;
        }

        public async Task<Result<bool>> Handle(AsignarPuertaCommand request, CancellationToken cancellationToken)
        {
            var vuelo = await _vueloRepository.ObtenerPorIdAsync(request.VueloId);
            if (vuelo == null)
                return Result<bool>.Failure("El vuelo no existe.", StatusCodes.Status404NotFound);

            if (vuelo.EstadoActual == EstadoVuelo.Cancelado || vuelo.EstadoActual == EstadoVuelo.Completado)
                return Result<bool>.Failure("No se puede cambiar la puerta de un vuelo cerrado o cancelado.", StatusCodes.Status400BadRequest);

            string puertaAnterior = vuelo.Puerta;
            var usuarioId = _seguridadService.ObtenerIdUsuarioActual();
            vuelo.ActualizarPuerta(request.NuevaPuerta, request.MotivoCambio, usuarioId);

            await _vueloRepository.ActualizarAsync(vuelo);

            var usuarioActual = _seguridadService.ObtenerUsarioActual();

            await _mediator.Publish(new VueloModificadoEvent
            {
                VueloId = vuelo.Id,
                NumeroVuelo = vuelo.NumeroVuelo,
                NuevoEstado = vuelo.EstadoActual.ToString(),
                Usuario = usuarioActual,
                MotivoCambio = $"Cambio de puerta de la {puertaAnterior} a la {request.NuevaPuerta}. Motivo: {request.MotivoCambio}",
                Accion = "Cambio de Puerta"
            }, cancellationToken);

            return Result<bool>.Success(true);
        }

    }
}