using MediatR;
using SIV.Application.Modulo.Aerolineas.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Aerolineas.Handlers
{
    public class EliminarAerolineaCommandHandler : IRequestHandler<EliminarAerolineaCommand, Result<bool>>
    {
        private readonly IAerolineaRepository _aerolineaRepository;

        public EliminarAerolineaCommandHandler(IAerolineaRepository aerolineaRepository)
        {
            _aerolineaRepository = aerolineaRepository;
        }

        public async Task<Result<bool>> Handle(EliminarAerolineaCommand request, CancellationToken cancellationToken)
        {
            var aerolinea = await _aerolineaRepository.ObtenerPorIdAsync(request.Id);
            if (aerolinea == null)
            {
                return Result<bool>.Failure($"No se encontró la aerolínea con Id {request.Id}");
            }

            await _aerolineaRepository.EliminarAsync(aerolinea);

            return Result<bool>.Success(true);
        }
    }
}