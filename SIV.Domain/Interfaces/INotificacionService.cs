using SIV.Domain.Entities.Vuelos;

namespace SIV.Domain.Interfaces
{
    public interface INotificacionService
    {
        Task EnviarCambioEstadoVueloAsync(Vuelo vuelo);
        Task EnviarAlertaUsuarioAsync(string usuarioId, string mensaje);
    }
}