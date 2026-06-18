namespace SIV.Application.Modulo.Auditoria.DTOs
{
    public class LogAuditoriaDto
    {
        public Guid Id { get; set; }
        public DateTime FechaHora { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Accion { get; set; } = string.Empty;
        public string Detalles { get; set; } = string.Empty;
        public string EntidadAfectada { get; set; } = string.Empty;
        public string EntidadId { get; set; } = string.Empty;
    }
}