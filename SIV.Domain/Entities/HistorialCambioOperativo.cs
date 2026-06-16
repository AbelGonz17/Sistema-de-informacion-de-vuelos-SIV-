using SIV.Domain.Common;

namespace SIV.Domain.Entities
{
    public class HistorialCambioOperativo
    {
        public Guid Id { get; set; }
        public Guid VueloId { get; set; }
        public TipoCambioOperativo TipoCambio { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string DetalleCambio { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public Guid UsuarioResponsable { get; set; }
    }
}