using MediatR;
using SIV.Application.Modulo.Vuelos.DTOs;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Vuelos.Queries
{
    public record ObtenerHistorialVueloQuery(Guid VueloId) : IRequest<Result<HistorialVueloDto>>;
}