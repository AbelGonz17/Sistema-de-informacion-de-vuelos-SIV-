using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Application.Modulo.Usuarios.Commands;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace SIV.Application.EventHandlers
{
    public class UsuarioSigueVueloNotificacionEmailHandler : INotificationHandler<UsuarioSigueVueloEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<UsuarioSigueVueloNotificacionEmailHandler> _logger;

        public UsuarioSigueVueloNotificacionEmailHandler(IEmailService emailService, ILogger<UsuarioSigueVueloNotificacionEmailHandler> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Handle(UsuarioSigueVueloEvent notification, CancellationToken cancellationToken)
        {
            // Solo enviar correo cuando inicia el seguimiento
            if (notification.Accion == "IniciarSeguimiento")
            {
                try
                {
                    var asunto = $"Has comenzado a seguir el vuelo {notification.NumeroVuelo}";
                    var cuerpo = $@"
                        <h2>¡Hola!</h2>
                        <p>Te confirmamos que has activado las notificaciones para el vuelo <strong>{notification.NumeroVuelo}</strong>.</p>
                        <p>A partir de ahora, te notificaremos por correo electrónico sobre cualquier actualización importante de este vuelo.</p>
                        <br/>
                        <p>Gracias por usar SistemaVuelos.</p>";

                    await _emailService.SendEmailAsync(notification.CorreoUsuario, asunto, cuerpo);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"No se pudo enviar el correo de confirmación de seguimiento a {notification.CorreoUsuario}. El proceso continuará.");
                }
            }
        }
    }
}
