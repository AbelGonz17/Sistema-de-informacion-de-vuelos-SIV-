using MediatR;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public class RegistrarCuentaCommand : IRequest<string>
    {
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
    }
}