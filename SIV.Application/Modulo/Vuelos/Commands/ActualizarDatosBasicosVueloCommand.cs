using MediatR;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Vuelos.Commands
{
    public record ActualizarDatosBasicosVueloCommand(
        Guid VueloId,
        Guid Aerolinea,
        Guid Origen,
        Guid Destino,
        DateTime HorarioPlanificadoSalida,
        DateTime HorarioPlanificadoLlegada,
        string Puerta,
        Guid UsuarioId
    ) : IRequest<Result<Guid>>;
}
