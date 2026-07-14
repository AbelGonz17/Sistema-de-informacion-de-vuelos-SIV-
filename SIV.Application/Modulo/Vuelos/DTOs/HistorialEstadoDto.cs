namespace SIV.Application.Modulo.Vuelos.DTOs
{
    public class HistorialEstadoDto
    {
        public string EstadoAnterior { get; set; } = string.Empty;
        public string EstadoNuevo { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public Guid UsuarioResponsable { get; set; }
    }
}