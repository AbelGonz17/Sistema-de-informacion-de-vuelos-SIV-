using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Application.Modulo.Usuarios.Commands;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace SIV.Application.EventHandlers
{
    public class UsuarioSigueVueloNotificacionEmailHandler : INotificationHandler<UsuarioSigueVueloEvent>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<UsuarioSigueVueloNotificacionEmailHandler> _logger;

        public UsuarioSigueVueloNotificacionEmailHandler(IServiceScopeFactory scopeFactory, ILogger<UsuarioSigueVueloNotificacionEmailHandler> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public Task Handle(UsuarioSigueVueloEvent notification, CancellationToken cancellationToken)
        {
            // Solo enviar correo cuando inicia el seguimiento
            if (notification.Accion == "IniciarSeguimiento")
            {
                // Enviar correo en segundo plano para no bloquear el flujo principal
                _ = Task.Run(async () =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                    try
                    {
                        var asunto = $"Has comenzado a seguir el vuelo {notification.NumeroVuelo}";
                        var cuerpo = $@"
                            <h2>¡Hola!</h2>
                            <p>Te confirmamos que has activado las notificaciones para el vuelo <strong>{notification.NumeroVuelo}</strong>.</p>
                            <p>A partir de ahora, te notificaremos por correo electrónico sobre cualquier actualización importante de este vuelo.</p>
                            <br/>
                            <p>Gracias por usar SistemaVuelos.</p>";

                        await emailService.SendEmailAsync(notification.CorreoUsuario, asunto, cuerpo);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"No se pudo enviar el correo de confirmación de seguimiento a {notification.CorreoUsuario} en segundo plano.");
                    }
                }, cancellationToken);
            }

            return Task.CompletedTask;
        }
    }
}
