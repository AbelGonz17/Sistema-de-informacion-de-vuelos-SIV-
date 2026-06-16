using SIV.Domain.Common;

namespace SIV.Domain.Entities
{
    public class Notificacion
    {
        public Guid Id { get; set; }
        public Guid UsuarioDestinatarioId { get; set; }
        public Guid VueloRelacionadoId { get; set; }
        public TipoEventoVuelo TipoEvento { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaHoraGenearicion { get; set; }
        public bool FueLeida { get; set; }
    }
}