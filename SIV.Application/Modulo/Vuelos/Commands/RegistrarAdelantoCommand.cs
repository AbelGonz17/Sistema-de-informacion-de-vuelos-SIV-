using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;
using System;

namespace SIV.Application.Modulo.Vuelos.Commands
{
    public record RegistrarAdelantoCommand(Guid VueloId, DateTime NuevaHoraSalida, string Motivo) 
        : IRequest<Result<bool>>, IComandoOperativo;
}
