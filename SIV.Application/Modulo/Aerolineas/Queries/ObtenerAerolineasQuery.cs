using MediatR;
using SIV.Application.Modulo.Aerolineas.DTOs;

namespace SIV.Application.Modulo.Aerolineas.Queries
{
    public record ObtenerAerolineasQuery : IRequest<IEnumerable<AerolineaDto>>;
}