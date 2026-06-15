using MediatR;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public record CambiarEstadoUsuarioCommand(
        Guid UsuarioId,
        bool Activo
    ) : IRequest<Result<bool>>;
}