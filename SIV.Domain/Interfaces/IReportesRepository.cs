using SIV.Domain.Entities.Vuelos;

namespace SIV.Domain.Interfaces
{
    public interface IReportesRepository
    {
        Task<IEnumerable<Vuelo>> ObtenerVuelosPorRangoFechaAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<IEnumerable<(HistorialCambioOperativo Cambio, string NumeroVuelo, string Operador)>> ObtenerCambiosOperativosAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<IEnumerable<(Guid VueloId, string NumeroVuelo, int CantidadSeguidores)>> ObtenerTopVuelosMasSeguidosAsync(int top);
        Task<int> ObtenerTotalUsuariosConSeguimientosActivosAsync();
    }
}