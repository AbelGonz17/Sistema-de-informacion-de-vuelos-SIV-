using MediatR;
using SIV.Application.Modulo.Aerolineas.DTOs;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Aerolineas.Queries
{
    public record ObtenerAerolineaPorIdQuery(Guid Id) : IRequest<Result<AerolineaDto>>;
}