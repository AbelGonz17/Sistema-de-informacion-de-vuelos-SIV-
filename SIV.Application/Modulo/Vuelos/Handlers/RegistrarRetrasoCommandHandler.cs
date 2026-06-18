using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Vuelos.Handlers
{
    public class RegistrarRetrasoCommandHandler : IRequestHandler<RegistrarRetrasoCommand, Result<bool>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly ISeguridadService _seguridadService;
        private readonly IMediator _mediator;

        public RegistrarRetrasoCommandHandler(
            IVueloRepository vueloRepository,
            ISeguridadService seguridadService,
            IMediator mediator)
        {
            _vueloRepository = vueloRepository;
            _seguridadService = seguridadService;
            _mediator = mediator;
        }

        public async Task<Result<bool>> Handle(RegistrarRetrasoCommand request, CancellationToken cancellationToken)
        {
            var vuelo = await _vueloRepository.ObtenerPorIdAsync(request.VueloId);

            if (vuelo == null)
                return Result<bool>.Failure("El vuelo especificado no existe.", StatusCodes.Status404NotFound);

            var usuarioId = _seguridadService.ObtenerIdUsuarioActual();
            vuelo.ActualizarHorarioEstimado(request.NuevaHoraSalida, request.Motivo, usuarioId);

            await _vueloRepository.ActualizarAsync(vuelo);

            var usuario = _seguridadService.ObtenerUsarioActual();

            string accionReal = request.NuevaHoraSalida > vuelo.HorarioPlanificadoSalida ? "RegistrarRetraso" : "RegistrarAdelanto";

            await _mediator.Publish(new VueloModificadoEvent
            {
                VueloId = vuelo.Id,
                NumeroVuelo = vuelo.NumeroVuelo,
                NuevoEstado = vuelo.EstadoActual.ToString(),
                MotivoCambio = $"Ajuste de horario para el vuelo {vuelo.NumeroVuelo}. Nueva hora estimada: {request.NuevaHoraSalida}. Motivo: {request.Motivo}",
                Usuario = usuario,
                Accion = accionReal
            }, cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}