using MediatR;
using SIV.Application.Modulo.Aerolineas.DTOs;
using SIV.Application.Modulo.Aerolineas.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Aerolineas.Handlers
{
    public class ObtenerAerolineaPorIdQueryHandler : IRequestHandler<ObtenerAerolineaPorIdQuery, Result<AerolineaDto>>
    {
        private readonly IAerolineaRepository _aerolineaRepository;

        public ObtenerAerolineaPorIdQueryHandler(IAerolineaRepository aerolineaRepository)
        {
            _aerolineaRepository = aerolineaRepository;
        }

        public async Task<Result<AerolineaDto>> Handle(ObtenerAerolineaPorIdQuery request, CancellationToken cancellationToken)
        {
            var aerolinea = await _aerolineaRepository.ObtenerPorIdAsync(request.Id);
            if (aerolinea == null)
            {
                return Result<AerolineaDto>.Failure($"No se encontró la aerolínea con Id {request.Id}");
            }

            var dto = new AerolineaDto
            {
                Id = aerolinea.Id,
                Codigo = aerolinea.Codigo,
                Nombre = aerolinea.Nombre
            };

            return Result<AerolineaDto>.Success(dto);
        }
    }
}