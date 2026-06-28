using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public record IniciarSeguimientoCommand(Guid UsuarioId, Guid VueloId) 
        : IRequest<Result<bool>>, IComandoAccionUsuario, IAuditableCommand
    {
        public string ObtenerMensajeAuditoria(object response)
        {
            if (response is Result<bool> result && result.IsSuccess)
            {
                return $"El usuario con ID {UsuarioId} comenzó a seguir el vuelo con ID {VueloId}.";
            }
            return $"Intento de seguir el vuelo {VueloId} por el usuario {UsuarioId} no fue completado.";
        }
    }
}