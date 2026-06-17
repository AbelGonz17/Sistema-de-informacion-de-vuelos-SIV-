using MediatR;
using SIV.Application.Modulo.Aeropuertos.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Modulo.Aeropuertos.Handlers
{
    public class EliminarAeropuertoCommandHandler : IRequestHandler<EliminarAeropuertoCommand, Result<bool>>
    {
        private readonly IAeropuertoRepository _aeropuertoRepository;

        public EliminarAeropuertoCommandHandler(IAeropuertoRepository aeropuertoRepository)
        {
            _aeropuertoRepository = aeropuertoRepository;
        }

        public async Task<Result<bool>> Handle(EliminarAeropuertoCommand request, CancellationToken cancellationToken)
        {
            var aeropuerto = await _aeropuertoRepository.ObtenerPorIdAsync(request.Id);
            if (aeropuerto == null)
            {
                return Result<bool>.Failure($"No se encontró el aeropuerto con Id {request.Id}");
            }

            await _aeropuertoRepository.EliminarAsync(aeropuerto);

            return Result<bool>.Success(true);
        }
    }
}
