using MediatR;

namespace SIV.Application.Modulo.Usuarios.Events
{
    public class UsuarioEstadoCambiadoEvent : INotification
    {
        public Guid UsuarioId { get; set; }
        public bool NuevoEstado { get; set; }
        public string UsuarioActor { get; set; } = string.Empty;
    }
}