using MediatR;
using SIV.Application.Modulo.Vuelos.DTOs;
using SIV.Domain.Common;
using System;

namespace SIV.Application.Modulo.Vuelos.Queries
{
    public record ObtenerDetalleVueloQuery(Guid VueloId) : IRequest<Result<VueloDetalleDto>>;
}
