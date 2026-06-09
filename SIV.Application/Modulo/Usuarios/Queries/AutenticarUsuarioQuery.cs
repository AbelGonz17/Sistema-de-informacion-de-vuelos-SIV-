using MediatR;

namespace SIV.Application.Modulo.Usuarios.Queries
{
    public class AutenticarUsuarioQuery : IRequest<string>
    {
        public string Correo { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
    }
}