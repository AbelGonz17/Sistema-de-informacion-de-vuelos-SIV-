using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public record DejarDeSeguirCommand(Guid UsuarioId, Guid VueloId) 
        : IRequest<Result<bool>>, IComandoAccionUsuario, IAuditableCommand
    {
        public string ObtenerMensajeAuditoria(object response)
        {
            if (response is Result<bool> result && result.IsSuccess)
            {
                return $"El usuario con ID {UsuarioId} dejó de seguir el vuelo con ID {VueloId}.";
            }
            return $"Intento de dejar de seguir el vuelo {VueloId} por el usuario {UsuarioId} no fue completado.";
        }
    }
}