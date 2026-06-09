using MediatR;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Vuelos.Handlers
{
    public class VueloModificadoEventHandler : INotificationHandler<VueloModificadoEvent>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly INotificacionService _notificacionService;

        public VueloModificadoEventHandler(IUsuarioRepository usuarioRepository, INotificacionService notificacionService)
        {
            _usuarioRepository = usuarioRepository;
            _notificacionService = notificacionService;
        }

        public async Task Handle(VueloModificadoEvent notification, CancellationToken cancellationToken)
        {
            var seguidoresCorreos = await _usuarioRepository.ObtenerSeguidoresDeVueloAsync(notification.VueloId);

            string mensajeAlerta = $"Atención: El vuelo {notification.NumeroVuelo} ha cambiado su estado a {notification.NuevoEstado}.";

            foreach (var correoUsuario in seguidoresCorreos)
            {
                await _notificacionService.EnviarAlertaUsuarioAsync(correoUsuario, mensajeAlerta);
            }
        }
    }
}