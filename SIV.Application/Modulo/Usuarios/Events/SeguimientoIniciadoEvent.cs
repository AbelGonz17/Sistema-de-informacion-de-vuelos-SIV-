using MediatR;

namespace SIV.Application.Modulo.Usuarios.Events
{
    public class SeguimientoIniciadoEvent : INotification
    {
        public Guid UsuarioId { get; set; }
        public Guid VueloId { get; set; }
        public string UsuarioActor { get; set; } = string.Empty;
    }
}