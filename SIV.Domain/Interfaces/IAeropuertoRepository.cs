using SIV.Domain.Entities;

namespace SIV.Domain.Interfaces
{
    public interface IAeropuertoRepository
    {
        Task<IEnumerable<Aeropuerto>> ObtenerTodosAsync();
        Task<Aeropuerto?> ObtenerPorIdAsync(Guid id);
        Task AgregarAsync(Aeropuerto aeropuerto);
        Task ActualizarAsync(Aeropuerto aeropuerto);
        Task EliminarAsync(Aeropuerto aeropuerto);
    }
}
