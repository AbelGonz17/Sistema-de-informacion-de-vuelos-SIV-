using SIV.Domain.Entities;

namespace SIV.Domain.Interfaces
{
    public interface INotificacionRepository
    {
        Task AgregarRangoAsync(IEnumerable<Notificacion> notificaciones);
        Task<IEnumerable<Notificacion>> ObtenerPorUsuarioAsync(Guid usuarioId);
        Task<Notificacion?> ObtenerPorIdAsync(Guid id);
        Task ActualizarAsync(Notificacion notificacion);
    }
}