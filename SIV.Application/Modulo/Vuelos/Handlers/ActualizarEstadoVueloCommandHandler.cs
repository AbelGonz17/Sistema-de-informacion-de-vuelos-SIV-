using MediatR;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Domain.Common;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Vuelos.Handlers
{
    public class ActualizarEstadoVueloCommandHandler : IRequestHandler<ActualizarEstadoVueloCommand, bool>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly IAuditoriaRepository _auditoriaRepository;
        private readonly ISeguridadService _seguridadService;
        private readonly IMediator _mediator;

        public  ActualizarEstadoVueloCommandHandler (
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

        public async Task<bool> Handle(ActualizarEstadoVueloCommand request, CancellationToken cancellationToken)
        {

            var vuelo = await _vueloRepository.ObtenerPorIdAsync(request.VueloId);

            if (request.NuevoEstado == EstadoVuelo.Retrasado)
            {
                vuelo.RegistrarRetraso(DateTime.UtcNow.AddHours(1), request.MotivoCambio);
            }
            else
            {
                vuelo.CambiarEstado(request.NuevoEstado);
            }

            await _vueloRepository.ActualizarAsync(vuelo);
            
            var usuarioActual = _seguridadService.ObtenerUsarioActual();
            var log = new LogAuditoria(
                Guid.NewGuid(),
                usuarioActual,
                "ActualizarEstadoVuelo",
                $"Se cambió el estado del vuelo {vuelo.NumeroVuelo} a {request.NuevoEstado}. Motivo: {request.MotivoCambio}"
            );
            await _auditoriaRepository.RegistrarLogAsync(log);

            await _mediator.Publish(new VueloModificadoEvent
            {
                VueloId = vuelo.Id,
                NumeroVuelo = vuelo.NumeroVuelo,
                NuevoEstado = vuelo.EstadoActual.ToString()
            }, cancellationToken);

            return true;
        }
    }
}