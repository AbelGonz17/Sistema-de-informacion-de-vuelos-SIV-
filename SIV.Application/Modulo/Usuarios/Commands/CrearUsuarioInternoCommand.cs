using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public record CrearUsuarioInternoCommand(
             string Nombre,
             string CorreoElectronico,
             string Contrasena,
             string Rol
         ) : IRequest<Result<string>>, IComandoCatalogo, IAuditableCommand
    {
        public string ObtenerMensajeAuditoria(object response)
        {
            if (response is Result<string> result && result.IsSuccess)
            {
                return $"Se creó exitosamente el usuario interno {Nombre} ({CorreoElectronico}) con el rol {Rol}.";
            }
            return $"Intento de crear el usuario interno {Nombre} ({CorreoElectronico}) con el rol {Rol} no fue completado.";
        }
    }
}
