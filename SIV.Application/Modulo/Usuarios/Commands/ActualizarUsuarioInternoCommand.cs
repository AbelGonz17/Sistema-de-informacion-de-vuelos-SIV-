using MediatR;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public record ActualizarUsuarioInternoCommand(Guid Id, string Nombre, string Rol) : IRequest<Result<string>>;
}