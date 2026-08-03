using MediatR;
using SIV.Application.Modulo.Aerolineas.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Aerolineas.Handlers
{
    public class EliminarAerolineaCommandHandler : IRequestHandler<EliminarAerolineaCommand, Result<bool>>
    {
        private readonly IAerolineaRepository _aerolineaRepository;
        private readonly IVueloRepository _vueloRepository;

        public EliminarAerolineaCommandHandler(IAerolineaRepository aerolineaRepository, IVueloRepository vueloRepository)
        {
            _aerolineaRepository = aerolineaRepository;
            _vueloRepository = vueloRepository;
        }

        public async Task<Result<bool>> Handle(EliminarAerolineaCommand request, CancellationToken cancellationToken)
        {
            var aerolinea = await _aerolineaRepository.ObtenerPorIdAsync(request.Id);

            if (aerolinea == null) 
                return Result<bool>.Failure($"No se encontró la aerolínea.");

            bool tieneVuelos = await _vueloRepository.ExistenVuelosActivosPorAerolineaAsync(request.Id);

            if (tieneVuelos) 
                return Result<bool>.Failure("No se puede desactivar la aerolínea porque tiene vuelos operativos activos.");

            await _aerolineaRepository.EliminarAsync(aerolinea);
            return Result<bool>.Success(true);
        }
    }
}