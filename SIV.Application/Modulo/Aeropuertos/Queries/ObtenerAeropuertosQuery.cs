using MediatR;
using SIV.Application.Modulo.Aeropuertos.DTOs;
using System.Collections.Generic;

namespace SIV.Application.Modulo.Aeropuertos.Queries
{
    public record ObtenerAeropuertosQuery : IRequest<IEnumerable<AeropuertoDto>>;
}
