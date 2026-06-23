using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;
using System;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public record DesactivarUsuarioCommand(Guid UsuarioId) : IRequest<Result<bool>>, IComandoCatalogo;
}
