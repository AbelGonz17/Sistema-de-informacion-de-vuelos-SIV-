using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;
using System;

namespace SIV.Application.Modulo.Aeropuertos.Commands
{
    public class CrearAeropuertoCommand : IRequest<Result<Guid>>, IComandoOperativo
    {
        public string Name { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        
        public Guid VueloId => Guid.Empty;
    }
}
