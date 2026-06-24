using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public record IniciarSeguimientoCommand(Guid UsuarioId, Guid VueloId) 
        : IRequest<Result<bool>>, IComandoAccionUsuario;
}