using MediatR;
using SIV.Domain.Common;
using SIV.Application.Modulo.Usuarios.DTOs;

namespace SIV.Application.Modulo.Usuarios.Queries
{
    public class AutenticarUsuarioQuery : IRequest<Result<TokenResponseDto>>
    {
        public string Correo { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
    }
}