using MediatR;
using SIV.Application.Modulo.Aerolineas.DTOs;
using SIV.Application.Modulo.Aerolineas.Queries;
using SIV.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Modulo.Aerolineas.Handlers
{
    public class ObtenerAerolineasQueryHandler : IRequestHandler<ObtenerAerolineasQuery, IEnumerable<AerolineaDto>>
    {
        private readonly IAerolineaRepository _aerolineaRepository;

        public ObtenerAerolineasQueryHandler(IAerolineaRepository aerolineaRepository)
        {
            _aerolineaRepository = aerolineaRepository;
        }

        public async Task<IEnumerable<AerolineaDto>> Handle(ObtenerAerolineasQuery request, CancellationToken cancellationToken)
        {
            var aerolineas = await _aerolineaRepository.ObtenerTodasAsync();

            return aerolineas.Select(a => new AerolineaDto
            {
                Id = a.Id,
                Codigo = a.Codigo,
                Nombre = a.Nombre
            });
        }
    }
}
