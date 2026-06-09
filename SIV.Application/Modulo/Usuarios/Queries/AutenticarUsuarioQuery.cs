using MediatR;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Usuarios.Queries
{
    public class AutenticarUsuarioQuery : IRequest<Result<string>>
    {
        public string Correo { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
    }
}