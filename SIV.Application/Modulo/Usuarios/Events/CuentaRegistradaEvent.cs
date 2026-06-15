using MediatR;

namespace SIV.Application.Modulo.Usuarios.Events
{
    public class CuentaRegistradaEvent : INotification
    {
        public Guid UsuarioId { get; set; }
        public string Correo { get; set; } = string.Empty;
    }
}