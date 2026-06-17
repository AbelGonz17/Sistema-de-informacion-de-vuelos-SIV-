using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Modulo.Vuelos.Handlers
{
    public class RegistrarAdelantoCommandHandler : IRequestHandler<RegistrarAdelantoCommand, Result<bool>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly ISeguridadService _seguridadService;
        private readonly IMediator _mediator;

        public RegistrarAdelantoCommandHandler(
            IVueloRepository vueloRepository,
            ISeguridadService seguridadService,
            IMediator mediator)
        {
            _vueloRepository = vueloRepository;
            _seguridadService = seguridadService;
            _mediator = mediator;
        }

        public async Task<Result<bool>> Handle(RegistrarAdelantoCommand request, CancellationToken cancellationToken)
        {
            var vuelo = await _vueloRepository.ObtenerPorIdAsync(request.VueloId);

            if (vuelo == null)
                return Result<bool>.Failure("El vuelo especificado no existe.", StatusCodes.Status404NotFound);

            var usuarioId = _seguridadService.ObtenerIdUsuarioActual();
            vuelo.ActualizarHorarioEstimado(request.NuevaHoraSalida, request.Motivo, usuarioId);

            await _vueloRepository.ActualizarAsync(vuelo);

            var usuarioNombre = _seguridadService.ObtenerUsarioActual();

            await _mediator.Publish(new VueloModificadoEvent
            {
                VueloId = vuelo.Id,
                NumeroVuelo = vuelo.NumeroVuelo,
                NuevoEstado = vuelo.EstadoActual.ToString(),
                MotivoCambio = $"Adelanto registrado para el vuelo {vuelo.NumeroVuelo}. Nueva hora estimada: {request.NuevaHoraSalida}. Motivo: {request.Motivo}",
                Usuario = usuarioNombre,
                Accion = "RegistrarAdelanto"
            }, cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
