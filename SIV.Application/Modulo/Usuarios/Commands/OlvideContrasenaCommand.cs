using MediatR;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public class OlvideContrasenaCommand : IRequest<Result<bool>>
    {
        public string CorreoElectronico { get; set; } = string.Empty;
        public string UrlBaseFrontend { get; set; } = string.Empty;
    }
}
