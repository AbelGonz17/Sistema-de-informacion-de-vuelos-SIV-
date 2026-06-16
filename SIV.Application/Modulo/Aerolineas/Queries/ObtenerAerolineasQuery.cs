using MediatR;
using SIV.Application.Modulo.Aerolineas.DTOs;
using System.Collections.Generic;

namespace SIV.Application.Modulo.Aerolineas.Queries
{
    public record ObtenerAerolineasQuery : IRequest<IEnumerable<AerolineaDto>>;
}
