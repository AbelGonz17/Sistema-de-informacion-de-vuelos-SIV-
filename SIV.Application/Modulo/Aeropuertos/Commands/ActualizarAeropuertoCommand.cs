using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Aeropuertos.Commands
{
    public record ActualizarAeropuertoCommand(string Codigo, string Nombre, string Pais) 
        : IRequest<Result<bool>>, IComandoCatalogo, IAuditableCommand
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public Guid Id { get; set; }

        public string ObtenerMensajeAuditoria(object response)
        {
            if (response is Result<bool> result && result.IsSuccess)
            {
                return $"Se actualizaron los datos del aeropuerto con ID {Id} a: {Nombre} ({Codigo}) en {Pais}.";
            }
            return $"Intento de actualizar el aeropuerto con ID {Id} no fue completado.";
        }
    }
}