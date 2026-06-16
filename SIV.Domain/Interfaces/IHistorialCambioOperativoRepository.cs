using SIV.Domain.Entities;

namespace SIV.Domain.Interfaces
{
    public interface IHistorialCambioOperativoRepository
    {
        Task<IEnumerable<HistorialCambioOperativo>> ObtenerPorVueloAsync(Guid vueloId);
        Task AgregarAsync(HistorialCambioOperativo historial);
    }
}
