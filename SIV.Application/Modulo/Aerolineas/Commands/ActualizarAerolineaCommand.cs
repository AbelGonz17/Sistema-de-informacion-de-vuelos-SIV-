using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Aerolineas.Commands
{
    public record ActualizarAerolineaCommand(string Codigo, string Nombre) 
        : IRequest<Result<bool>>, IComandoCatalogo, IAuditableCommand
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public Guid Id { get; set; }

        public string ObtenerMensajeAuditoria(object response)
        {
            if (response is Result<bool> result && result.IsSuccess)
            {
                return $"Se actualizaron los datos de la aerolínea con ID {Id} a: {Nombre} ({Codigo}).";
            }
            return $"Intento de actualizar la aerolínea con ID {Id} no fue completado.";
        }
    }
}