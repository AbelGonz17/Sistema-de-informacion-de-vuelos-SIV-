using MediatR;
using SIV.Application.Modulo.Aeropuertos.DTOs;
using SIV.Application.Modulo.Aeropuertos.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Modulo.Aeropuertos.Handlers
{
    public class ObtenerAeropuertoPorIdQueryHandler : IRequestHandler<ObtenerAeropuertoPorIdQuery, Result<AeropuertoDto>>
    {
        private readonly IAeropuertoRepository _aeropuertoRepository;

        public ObtenerAeropuertoPorIdQueryHandler(IAeropuertoRepository aeropuertoRepository)
        {
            _aeropuertoRepository = aeropuertoRepository;
        }

        public async Task<Result<AeropuertoDto>> Handle(ObtenerAeropuertoPorIdQuery request, CancellationToken cancellationToken)
        {
            var aeropuerto = await _aeropuertoRepository.ObtenerPorIdAsync(request.Id);
            if (aeropuerto == null)
            {
                return Result<AeropuertoDto>.Failure($"No se encontró el aeropuerto con Id {request.Id}");
            }

            var dto = new AeropuertoDto
            {
                Id = aeropuerto.Id,
                Codigo = aeropuerto.Codigo,
                Nombre = aeropuerto.Nombre,
                Pais = aeropuerto.Pais
            };

            return Result<AeropuertoDto>.Success(dto);
        }
    }
}
