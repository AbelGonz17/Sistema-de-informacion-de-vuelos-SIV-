using MediatR;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public class RestablecerContrasenaCommand : IRequest<Result<bool>>
    {
        public string Token { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public string NuevaContrasena { get; set; } = string.Empty;
    }
}
