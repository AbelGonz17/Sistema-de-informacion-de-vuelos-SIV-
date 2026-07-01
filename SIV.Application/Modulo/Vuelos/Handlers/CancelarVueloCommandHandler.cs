using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Domain.Common;
using SIV.Domain.Entities.Vuelos;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Vuelos.Handlers
{
    public class CancelarVueloCommandHandler : IRequestHandler<CancelarVueloCommand, Result<bool>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly ISeguridadService _seguridadService;
        private readonly IMediator _mediator;

        public CancelarVueloCommandHandler(
            IVueloRepository vueloRepository,
            ISeguridadService seguridadService,
            IMediator mediator)
        {
            _vueloRepository = vueloRepository;
            _seguridadService = seguridadService;
            _mediator = mediator;
        }

        public async Task<Result<bool>> Handle(CancelarVueloCommand request, CancellationToken cancellationToken)
        {
            var vuelo = await _vueloRepository.ObtenerPorIdAsync(request.VueloId);

            if (vuelo == null)
                return Result<bool>.Failure("El vuelo especificado no existe.", StatusCodes.Status404NotFound);

            var usuarioId = _seguridadService.ObtenerIdUsuarioActual();
            vuelo.CambiarEstado(EstadoVuelo.Cancelado, request.Motivo, usuarioId);

            await _vueloRepository.ActualizarAsync(vuelo);

            var usuarioNombre = _seguridadService.ObtenerUsarioActual();

            await _mediator.Publish(new VueloModificadoEvent
            {
                VueloId = vuelo.Id,
                NumeroVuelo = vuelo.NumeroVuelo,
                NuevoEstado = vuelo.EstadoActual.ToString(),
                MotivoCambio = $"Cancelación del vuelo {vuelo.NumeroVuelo}. Motivo: {request.Motivo}",
                Usuario = usuarioNombre,
                Accion = "CancelarVuelo"
            }, cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}