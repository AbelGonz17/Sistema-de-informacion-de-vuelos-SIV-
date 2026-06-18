namespace SIV.Application.Modulo.Vuelos.DTOs
{
    public class HistorialVueloDto
    {
        public Guid VueloId { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public IEnumerable<HistorialEstadoDto> HistorialEstados { get; set; } = new List<HistorialEstadoDto>();
        public IEnumerable<HistorialCambioOperativoDto> HistorialCambios { get; set; } = new List<HistorialCambioOperativoDto>();
    }
}