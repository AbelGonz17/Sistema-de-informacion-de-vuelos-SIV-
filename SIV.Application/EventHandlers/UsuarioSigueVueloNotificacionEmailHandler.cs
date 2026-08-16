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
            if (notification.Accion != "IniciarSeguimiento")
                return Task.CompletedTask;

            // Capturar los datos necesarios antes de salir del scope de la request
            var correo = notification.CorreoUsuario;
            var numeroVuelo = notification.NumeroVuelo;

            // IMPORTANTE: usar CancellationToken.None para que el background task
            // NO se cancele cuando la petición HTTP termina.
            _ = Task.Run(async () =>
            {
                try
                {
                    _logger.LogInformation($"[Email] Iniciando envío de correo de confirmación a {correo} para vuelo {numeroVuelo}");

                    using var scope = _scopeFactory.CreateScope();
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                    var asunto = $"Has comenzado a seguir el vuelo {numeroVuelo}";
                    var cuerpo = $@"
                        <h2>¡Hola!</h2>
                        <p>Te confirmamos que has activado las notificaciones para el vuelo <strong>{numeroVuelo}</strong>.</p>
                        <p>A partir de ahora, te notificaremos por correo electrónico sobre cualquier actualización importante de este vuelo.</p>
                        <br/>
                        <p>Gracias por usar SistemaVuelos.</p>";

                    await emailService.SendEmailAsync(correo, asunto, cuerpo);

                    _logger.LogInformation($"[Email] Correo de confirmación enviado exitosamente a {correo}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"[Email] Error al enviar correo de confirmación a {correo}");
                }
            }, CancellationToken.None); // <-- CancellationToken.None, nunca ligado a la request HTTP

            return Task.CompletedTask;
        }
    }
}
