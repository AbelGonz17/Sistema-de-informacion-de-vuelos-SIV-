using System;

namespace SIV.Application.Modulo.Vuelos.DTOs
{
    public class HistorialCambioOperativoDto
    {
        public Guid Id { get; set; }
        public string TipoCambio { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public string DetalleCambio { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public Guid UsuarioResponsable { get; set; }
    }
}
