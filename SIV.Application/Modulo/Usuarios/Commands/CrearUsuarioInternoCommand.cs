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
         ) : IRequest<Result<string>>, IComandoCatalogo;
}
