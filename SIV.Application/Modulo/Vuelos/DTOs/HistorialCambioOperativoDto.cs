namespace SIV.Application.Modulo.Vuelos.DTOs
{
    public class HistorialCambioOperativoDto
    {
        public string TipoCambio { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public string DetalleCambio { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public string UsuarioResponsable { get; set; } = string.Empty;
    }
}