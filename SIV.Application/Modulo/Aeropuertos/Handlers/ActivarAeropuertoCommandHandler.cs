using MediatR;
using SIV.Application.Modulo.Aeropuertos.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Aeropuertos.Handlers
{
    public class ActivarAeropuertoCommandHandler : IRequestHandler<ActivarAeropuertoCommand, Result<bool>>
    {
        private readonly IAeropuertoRepository _aeropuertoRepository;

        public ActivarAeropuertoCommandHandler(IAeropuertoRepository aeropuertoRepository)
        {
            _aeropuertoRepository = aeropuertoRepository;
        }

        public async Task<Result<bool>> Handle(ActivarAeropuertoCommand request, CancellationToken cancellationToken)
        {
            var aeropuerto = await _aeropuertoRepository.ObtenerPorIdAsync(request.Id);
            if (aeropuerto == null)
            {
                return Result<bool>.Failure("Aeropuerto no encontrado.");
            }

            aeropuerto.Activar();
            
            await _aeropuertoRepository.ActualizarAsync(aeropuerto);
            
            return Result<bool>.Success(true);
        }
    }
}
