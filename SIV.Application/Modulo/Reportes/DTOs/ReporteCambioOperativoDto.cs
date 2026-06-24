namespace SIV.Application.Modulo.Reportes.DTOs
{
    public class ReporteCambioOperativoDto
    {
        public Guid CambioId { get; set; }
        public Guid VueloId { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public string TipoCambio { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public string DetalleCambio { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public string OperadorResponsable { get; set; } = string.Empty;
    }
}
