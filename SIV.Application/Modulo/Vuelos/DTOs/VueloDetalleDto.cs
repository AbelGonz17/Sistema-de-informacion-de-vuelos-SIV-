namespace SIV.Application.Modulo.Vuelos.DTOs
{
    public record VueloDetalleDto(
        Guid Id,
        string NumeroVuelo,
        string Aerolinea,
        string Origen,
        string Destino,
        DateTime HorarioPlanificadoSalida,
        DateTime HorarioPlanificadoLlegada,
        DateTime? HorarioEstimadoSalida,
        DateTime? HorarioEstimadoLlegada,
        string Puerta,
        string EstadoActual,
        string MotivoUltimoCambio,
        List<HistorialEstadoDto> HistorialEstados,
        List<HistorialCambioOperativoDto> HistorialCambio
    );
}
