using MediatR;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public record CerrarSesionCommand(Guid UsuarioId, string? RefreshToken) : IRequest<Result<bool>>;
}
