using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Aeropuertos.Commands
{
    public record ActualizarAeropuertoCommand(string Codigo, string Nombre, string Pais) 
        : IRequest<Result<bool>>, IComandoCatalogo
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public Guid Id { get; set; }
    }
}