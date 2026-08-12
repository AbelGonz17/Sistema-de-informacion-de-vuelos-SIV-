using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Domain.Interfaces;
using System.Linq;

namespace SIV.Application.EventHandlers
{
    public class VueloModificadoEventHandler : INotificationHandler<VueloModificadoEvent>
    {
        private readonly INotificacionService _notificacionService;
        private readonly IVueloRepository _vueloRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmailService _emailService;

        public VueloModificadoEventHandler(
            INotificacionService notificacionService, 
            IVueloRepository vueloRepository,
            IUsuarioRepository usuarioRepository,
            IEmailService emailService)
        {
            _notificacionService = notificacionService;
            _vueloRepository = vueloRepository;
            _usuarioRepository = usuarioRepository;
            _emailService = emailService;
        }

        public async Task Handle(VueloModificadoEvent notification, CancellationToken cancellationToken)
        {
            var vuelo = await _vueloRepository.ObtenerPorIdAsync(notification.VueloId);

            if (vuelo != null)
            {
                // Notificación en tiempo real (SignalR)
                await _notificacionService.EnviarCambioEstadoVueloAsync(vuelo);

                // Notificación por correo electrónico
                var seguidoresCorreos = await _usuarioRepository.ObtenerSeguidoresDeVueloAsync(vuelo.Id);
                if (seguidoresCorreos.Any())
                {
                    var asunto = $"Actualización de estado: Vuelo {vuelo.NumeroVuelo}";
                    var cuerpo = $@"
                        <h2>Actualización de Vuelo</h2>
                        <p>El vuelo <strong>{vuelo.NumeroVuelo}</strong> que estás siguiendo ha presentado una actualización importante.</p>
                        <p><strong>Estado Actual:</strong> {vuelo.EstadoActual}</p>
                        <p><strong>Puerta:</strong> {vuelo.Puerta}</p>
                        <p><strong>Motivo / Detalle:</strong> {vuelo.MotivoUltimoCambio ?? "Sin detalles adicionales"}</p>
                        <br/>
                        <p>Por favor, revisa la plataforma para ver los detalles en tiempo real.</p>";

                    foreach (var correo in seguidoresCorreos)
                    {
                        await _emailService.SendEmailAsync(correo, asunto, cuerpo);
                    }
                }
            }
        }
    }
}