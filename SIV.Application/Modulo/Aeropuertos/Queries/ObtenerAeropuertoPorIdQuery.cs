using MediatR;
using SIV.Application.Modulo.Aeropuertos.DTOs;
using SIV.Domain.Common;
using System;

namespace SIV.Application.Modulo.Aeropuertos.Queries
{
    public record ObtenerAeropuertoPorIdQuery(Guid Id) : IRequest<Result<AeropuertoDto>>;
}
