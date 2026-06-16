using SIV.Domain.Entities;

namespace SIV.Domain.Interfaces
{
    public interface IHistorialEstadoRepository
    {
        Task<IEnumerable<HistorialEstado>> ObtenerPorVueloAsync(Guid vueloId);
        Task AgregarAsync(HistorialEstado historial);
    }
}
