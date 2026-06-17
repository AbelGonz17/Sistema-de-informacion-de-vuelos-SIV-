using System;

namespace SIV.Application.Modulo.Usuarios.DTOs
{
    public class NotificacionDto
    {
        public Guid Id { get; set; }
        public Guid VueloRelacionadoId { get; set; }
        public string TipoEvento { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaHoraGenearicion { get; set; }
        public bool FueLeida { get; set; }
    }
}
