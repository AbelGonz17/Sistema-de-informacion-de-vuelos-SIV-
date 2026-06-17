using MediatR;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Vuelos.Handlers
{
    public class VueloModificadoEventHandler : INotificationHandler<VueloModificadoEvent>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly INotificacionService _notificacionService;
        private readonly INotificacionRepository _notificacionRepository;

        public VueloModificadoEventHandler(
            IUsuarioRepository usuarioRepository, 
            INotificacionService notificacionService,
            INotificacionRepository notificacionRepository)
        {
            _usuarioRepository = usuarioRepository;
            _notificacionService = notificacionService;
            _notificacionRepository = notificacionRepository;
        }

        public async Task Handle(VueloModificadoEvent notification, CancellationToken cancellationToken)
        {
            var seguidoresCorreos = await _usuarioRepository.ObtenerSeguidoresDeVueloAsync(notification.VueloId);
            var seguidoresIds = await _usuarioRepository.ObtenerIdsSeguidoresDeVueloAsync(notification.VueloId);

            string mensajeAlerta = $"Atención: El vuelo {notification.NumeroVuelo} ha cambiado su estado a {notification.NuevoEstado}.";

            var notificaciones = new List<Notificacion>();

            foreach (var usuarioId in seguidoresIds)
            {
                notificaciones.Add(new Notificacion
                {
                    Id = Guid.NewGuid(),
                    UsuarioDestinatarioId = usuarioId,
                    VueloRelacionadoId = notification.VueloId,
                    TipoEvento = SIV.Domain.Common.TipoEventoVuelo.CambioEstado, 
                    Mensaje = mensajeAlerta,
                    FechaHoraGenearicion = DateTime.UtcNow,
                    FueLeida = false
                });
            }

            if (notificaciones.Any())
            {
                await _notificacionRepository.AgregarRangoAsync(notificaciones);
            }

            foreach (var correoUsuario in seguidoresCorreos)
            {
                await _notificacionService.EnviarAlertaUsuarioAsync(correoUsuario, mensajeAlerta);
            }
        }
    }
}