using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;
using System;

namespace SIV.Application.Modulo.Aerolineas.Commands
{
    public class CrearAerolineaCommand : IRequest<Result<Guid>>, IComandoOperativo
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        
        public Guid VueloId => Guid.Empty;
    }
}
