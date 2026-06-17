using MediatR;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Vuelos.Commands
{
    public record CrearVueloCommand(
        string NumeroVuelo, 
        Guid Aerolinea, 
        Guid Origen, 
        Guid Destino, 
        DateTime HorarioPlanificadoSalida, 
        DateTime HorarioPlanificadoLlegada, 
        string Puerta
    ) : IRequest<Result<Guid>>, IComandoCatalogo;
}