using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;
using System;

namespace SIV.Application.Modulo.Vuelos.Commands
{
    public record CancelarVueloCommand(Guid VueloId, string Motivo) 
        : IRequest<Result<bool>>, IComandoOperativo;
}
