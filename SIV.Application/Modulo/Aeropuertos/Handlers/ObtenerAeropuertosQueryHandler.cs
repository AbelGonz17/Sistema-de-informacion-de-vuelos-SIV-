using MediatR;
using SIV.Application.Modulo.Aeropuertos.DTOs;
using SIV.Application.Modulo.Aeropuertos.Queries;
using SIV.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Modulo.Aeropuertos.Handlers
{
    public class ObtenerAeropuertosQueryHandler : IRequestHandler<ObtenerAeropuertosQuery, IEnumerable<AeropuertoDto>>
    {
        private readonly IAeropuertoRepository _aeropuertoRepository;

        public ObtenerAeropuertosQueryHandler(IAeropuertoRepository aeropuertoRepository)
        {
            _aeropuertoRepository = aeropuertoRepository;
        }

        public async Task<IEnumerable<AeropuertoDto>> Handle(ObtenerAeropuertosQuery request, CancellationToken cancellationToken)
        {
            var aeropuertos = await _aeropuertoRepository.ObtenerTodosAsync();

            return aeropuertos.Select(a => new AeropuertoDto
            {
                Id = a.Id,
                Codigo = a.Codigo,
                Nombre = a.Nombre,
                Pais = a.Pais
            });
        }
    }
}
