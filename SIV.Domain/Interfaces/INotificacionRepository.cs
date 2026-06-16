using SIV.Domain.Entities;

namespace SIV.Domain.Interfaces
{
    public interface INotificacionRepository
    {
        Task<IEnumerable<Notificacion>> ObtenerPorUsuarioAsync(Guid usuarioId);
        Task AgregarAsync(Notificacion notificacion);
        Task AgregarRangoAsync(IEnumerable<Notificacion> notificaciones);
        Task MarcarComoLeidasAsync(IEnumerable<Guid> notificacionIds);
    }
}
