using MediatR;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Domain.Common;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Vuelos.Handlers
{
    public class RegistrarRetrasoCommandHandler : IRequestHandler<RegistrarRetrasoCommand, Result<bool>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly IAuditoriaRepository _auditoriaRepository;
        private readonly ISeguridadService _seguridadService;
        private readonly IMediator _mediator;

        public RegistrarRetrasoCommandHandler(
            IVueloRepository vueloRepository,
            IAuditoriaRepository auditoriaRepository,
            ISeguridadService seguridadService,
            IMediator mediator)
        {
            _vueloRepository = vueloRepository;
            _auditoriaRepository = auditoriaRepository;
            _seguridadService = seguridadService;
            _mediator = mediator;
        }

        public async Task<Result<bool>> Handle(RegistrarRetrasoCommand request, CancellationToken cancellationToken)
        {   
            var vuelo = await _vueloRepository.ObtenerPorIdAsync(request.VueloId);

            vuelo.RegistrarRetraso(request.NuevaHoraSalida, request.Motivo);

            await _vueloRepository.ActualizarAsync(vuelo);

            var usuario = _seguridadService.ObtenerUsarioActual();
            var log = new LogAuditoria(
                Guid.NewGuid(),
                usuario,
                "RegistrarRetraso",
                $"Retraso registrado para el vuelo {vuelo.NumeroVuelo}. Nueva hora: {request.NuevaHoraSalida}. Motivo: {request.Motivo}"
            );
            await _auditoriaRepository.RegistrarLogAsync(log);

            await _mediator.Publish(new VueloModificadoEvent
            {
                VueloId = vuelo.Id,
                NumeroVuelo = vuelo.NumeroVuelo,
                NuevoEstado = vuelo.EstadoActual.ToString()
            }, cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}