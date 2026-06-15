using MediatR;
using SIV.Application.Modulo.Vuelos.Events;
using SIV.Domain.Interfaces;

namespace SIV.Application.EventHandlers
{
    public class VueloModificadoEventHandler : INotificationHandler<VueloModificadoEvent>
    {
        private readonly INotificacionService _notificacionService;
        private readonly IVueloRepository _vueloRepository;

        public VueloModificadoEventHandler(INotificacionService notificacionService, IVueloRepository vueloRepository)
        {
            _notificacionService = notificacionService;
            _vueloRepository = vueloRepository;
        }

        public async Task Handle(VueloModificadoEvent notification, CancellationToken cancellationToken)
        {
            var vuelo = await _vueloRepository.ObtenerPorIdAsync(notification.VueloId);

            if (vuelo != null)
            {
                await _notificacionService.EnviarCambioEstadoVueloAsync(vuelo);
            }
        }
    }
}