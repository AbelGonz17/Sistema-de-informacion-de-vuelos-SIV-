using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Aerolineas.Commands
{
    public record ActualizarAerolineaCommand(string Codigo, string Nombre) 
        : IRequest<Result<bool>>, IComandoCatalogo
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public Guid Id { get; set; }
    }
}