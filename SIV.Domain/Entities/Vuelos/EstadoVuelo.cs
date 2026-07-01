namespace SIV.Domain.Entities.Vuelos
{
    public enum EstadoVuelo
    {
        Programado,
        Embarcando,
        EnVuelo,
        Aterrizado,
        Completado,
        Cancelado,
        Retrasado,
        Adelantado
    }

    public enum TipoEventoVuelo
    {
        CambioEstado,
        CambioOperativo
    }
}