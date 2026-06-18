using SIV.Domain.Entities;

namespace SIV.Domain.Interfaces
{
    public interface IAerolineaRepository
    {
        Task<IEnumerable<Aerolinea>> ObtenerTodasAsync();
        Task<Aerolinea?> ObtenerPorCodigoAsync(string codigo);
        Task<Aerolinea?> ObtenerPorIdAsync(Guid id);
        Task AgregarAsync(Aerolinea aerolinea);
        Task ActualizarAsync(Aerolinea aerolinea);
        Task EliminarAsync(Aerolinea aerolinea);
        Task<bool> ExisteCodigoParaOtraAerolineaAsync(Guid idActual, string codigo);
    }
}