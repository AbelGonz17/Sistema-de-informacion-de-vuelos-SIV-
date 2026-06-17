using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Vuelos.Commands
{
    public record RegistrarRetrasoCommand(Guid VueloId, DateTime NuevaHoraSalida, string Motivo) 
        : IRequest<Result<bool>>, IComandoOperativo;
}