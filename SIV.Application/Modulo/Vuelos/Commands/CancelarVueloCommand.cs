using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Vuelos.Commands
{
    public record CancelarVueloCommand(Guid VueloId, string Motivo) 
        : IRequest<Result<bool>>, IComandoOperativo, IAuditableCommand
    {
        public string ObtenerMensajeAuditoria(object response)
        {
            if (response is Result<bool> result && result.IsSuccess)
            {
                return $"Se canceló de manera definitiva el vuelo con ID {VueloId}. Motivo: {Motivo}.";
            }
            return $"Intento de cancelar el vuelo con ID {VueloId} no fue completado.";
        }
    }
}