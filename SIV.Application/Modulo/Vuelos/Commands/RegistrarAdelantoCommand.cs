using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Vuelos.Commands
{
    public record RegistrarAdelantoCommand(Guid VueloId, DateTime NuevaHoraSalida, string Motivo) 
        : IRequest<Result<bool>>, IComandoOperativo, IAuditableCommand
    {
        public string ObtenerMensajeAuditoria(object response)
        {
            if (response is Result<bool> result && result.IsSuccess)
            {
                return $"Se registró un adelanto para el vuelo con ID {VueloId}. Nuevo horario estimado de salida: {NuevaHoraSalida:yyyy-MM-dd HH:mm:ss}. Motivo: {Motivo}.";
            }
            return $"Intento de registrar adelanto para el vuelo con ID {VueloId} no fue completado.";
        }
    }
}