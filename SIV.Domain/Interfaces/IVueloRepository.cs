using SIV.Domain.Entities.Vuelos;

namespace SIV.Domain.Interfaces
{
    public interface IVueloRepository
    {
        Task<Vuelo?> ObtenerPorIdAsync(Guid id);
        Task<Vuelo?> ObtenerPorIdConHistorialAsync(Guid id);
        Task<Vuelo?> ObtenerDetalleCompletoAsync(Guid id);
        Task<bool> ExistenVuelosActivosPorAerolineaAsync(Guid aerolineaId);
        Task<Vuelo?> ObtenerPorNumeroAsync(string numeroVuelo);
        Task<IEnumerable<Vuelo?>> ObtenerVuelosPorFechaYTipoAsync(DateTime fecha, bool esLlegada); 
        Task AgregarAsync(Vuelo vuelo);
        Task ActualizarAsync(Vuelo vuelo);
        Task<bool> ExisteVueloAsync(string numeroVuelo, Guid aerolinea, DateTime fecha, Guid origen, Guid destino);
        Task<(IEnumerable<Vuelo?> Vuelos, int TotalCount)> ObtenerVuelosFidsPaginadosAsync(int pageNumber, int pageSize, bool? esLlegada, string? estado, Guid? aerolineaId, DateTime? fecha);
        Task<IEnumerable<Vuelo?>> ObtenerTodosAsync();
    }
}