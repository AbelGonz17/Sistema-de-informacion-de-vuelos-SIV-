using SIV.Domain.Entities;

namespace SIV.Domain.Interfaces
{
    public interface IAerolineaRepository
    {
        Task<IEnumerable<Aerolinea>> ObtenerTodasAsync();
        Task<Aerolinea?> ObtenerPorIdAsync(Guid id);
        Task<Aerolinea?> ObtenerPorCodigoAsync(string codigo);
        Task AgregarAsync(Aerolinea aerolinea);
        Task ActualizarAsync(Aerolinea aerolinea);
        Task EliminarAsync(Aerolinea aerolinea);
    }
}
