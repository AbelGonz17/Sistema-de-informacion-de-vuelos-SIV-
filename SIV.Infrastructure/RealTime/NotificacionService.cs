using Microsoft.AspNetCore.SignalR;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;

namespace SIV.Infrastructure.RealTime
{
    public class NotificacionService : INotificacionService
    {
        private readonly IHubContext<FidsHub> _hubContext;

        public NotificacionService(IHubContext<FidsHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task EnviarCambioEstadoVueloAsync(Vuelo vuelo)
        {
            var payload = new
            {
                vueloId = vuelo.Id,
                numeroVuelo = vuelo.NumeroVuelo,
                aerolinea = vuelo.Aerolinea,
                origen = vuelo.Origen,
                destino = vuelo.Destino,
                horarioPlanificado = vuelo.HorarioPlanificadoSalida,
                horarioEstimado = vuelo.HorarioEstimadoSalida,
                puerta = vuelo.Puerta,
                estadoActual = vuelo.EstadoActual.ToString()
            };

            await _hubContext.Clients.All.SendAsync("RecibirCambioVuelo", payload);
        }

        public async Task EnviarAlertaUsuarioAsync(string usuarioId, string mensaje)
        {
            await _hubContext.Clients.User(usuarioId).SendAsync("RecibirAlertaPersonalizada", mensaje);
        }
    }
}