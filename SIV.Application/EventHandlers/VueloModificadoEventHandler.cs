using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.EventHandlers
{
    public class VueloModificadoEventHandler : INotificationHandler<VueloModificadoEvent>
    {
        private readonly INotificacionService _notificacionService;
        private readonly IVueloRepository _vueloRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<VueloModificadoEventHandler> _logger;

        public VueloModificadoEventHandler(
            INotificacionService notificacionService, 
            IVueloRepository vueloRepository,
            IUsuarioRepository usuarioRepository,
            IServiceScopeFactory scopeFactory,
            ILogger<VueloModificadoEventHandler> logger)
        {
            _notificacionService = notificacionService;
            _vueloRepository = vueloRepository;
            _usuarioRepository = usuarioRepository;
            _scopeFactory = scopeFactory;
            _logger = logger;
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
                    // Obtener datos necesarios para el correo antes de salir del hilo/scope actual
                    var numeroVuelo = vuelo.NumeroVuelo;
                    var estadoActual = vuelo.EstadoActual;
                    var puerta = vuelo.Puerta;
                    var motivo = vuelo.MotivoUltimoCambio ?? "Sin detalles adicionales";

                    // Enviar correos en segundo plano para no bloquear al operador
                    _ = Task.Run(async () =>
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                        var asunto = $"Actualización de estado: Vuelo {numeroVuelo}";
                        var cuerpo = $@"
                            <h2>Actualización de Vuelo</h2>
                            <p>El vuelo <strong>{numeroVuelo}</strong> que estás siguiendo ha presentado una actualización importante.</p>
                            <p><strong>Estado Actual:</strong> {estadoActual}</p>
                            <p><strong>Puerta:</strong> {puerta}</p>
                            <p><strong>Motivo / Detalle:</strong> {motivo}</p>
                            <br/>
                            <p>Por favor, revisa la plataforma para ver los detalles en tiempo real.</p>";

                        foreach (var correo in seguidoresCorreos)
                        {
                            try
                            {
                                await emailService.SendEmailAsync(correo, asunto, cuerpo);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"Error al enviar correo de actualización de vuelo a {correo} en segundo plano.");
                            }
                        }
                    }, cancellationToken);
                }
            }
        }
    }
}