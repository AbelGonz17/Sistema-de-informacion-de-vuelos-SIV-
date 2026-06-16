namespace SIV.Domain.Common
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