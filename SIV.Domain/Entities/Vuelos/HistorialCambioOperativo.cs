using SIV.Domain.Common;

namespace SIV.Domain.Entities.Vuelos
{
    public class HistorialCambioOperativo
    {
        public Guid Id { get; set; }
        public Guid VueloId { get; set; }
        public string TipoCambio { get; set; } // O crear enum TipoCambioOperativo
        public string Motivo { get; set; } = string.Empty;
        public string DetalleCambio { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public Guid UsuarioResponsable { get; set; }
    }
}