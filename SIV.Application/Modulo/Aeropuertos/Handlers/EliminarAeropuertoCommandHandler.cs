using MediatR;
using SIV.Application.Modulo.Aeropuertos.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Aeropuertos.Handlers
{
    public class EliminarAeropuertoCommandHandler : IRequestHandler<EliminarAeropuertoCommand, Result<bool>>
    {
        private readonly IAeropuertoRepository _aeropuertoRepository;
        private readonly IVueloRepository _vueloRepository;

        public EliminarAeropuertoCommandHandler(IAeropuertoRepository aeropuertoRepository, IVueloRepository vueloRepository)
        {
            _aeropuertoRepository = aeropuertoRepository;
            _vueloRepository = vueloRepository;
        }

        public async Task<Result<bool>> Handle(EliminarAeropuertoCommand request, CancellationToken cancellationToken)
        {
            var aeropuerto = await _aeropuertoRepository.ObtenerPorIdAsync(request.Id);
            if (aeropuerto == null)
            {
                return Result<bool>.Failure("No se encontró el aeropuerto.");
            }

            bool tieneVuelos = await _vueloRepository.ExistenVuelosActivosPorAeropuertoAsync(request.Id);
            if (tieneVuelos)
            {
                return Result<bool>.Failure("No se puede desactivar el aeropuerto porque tiene vuelos operativos activos.");
            }

            await _aeropuertoRepository.EliminarAsync(aeropuerto);

            return Result<bool>.Success(true);
        }
    }
}