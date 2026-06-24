using MediatR;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public class UsuarioSigueVueloEvent : INotification
    {
        public string CorreoUsuario { get; set; } = string.Empty;
        public string NumeroVuelo { get; set; } = string.Empty;
        public string Accion { get; set; } = string.Empty;
    }
}
