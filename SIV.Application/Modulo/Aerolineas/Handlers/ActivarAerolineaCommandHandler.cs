using MediatR;
using SIV.Application.Modulo.Aerolineas.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Aerolineas.Handlers
{
    public class ActivarAerolineaCommandHandler : IRequestHandler<ActivarAerolineaCommand, Result<bool>>
    {
        private readonly IAerolineaRepository _aerolineaRepository;

        public ActivarAerolineaCommandHandler(IAerolineaRepository aerolineaRepository)
        {
            _aerolineaRepository = aerolineaRepository;
        }

        public async Task<Result<bool>> Handle(ActivarAerolineaCommand request, CancellationToken cancellationToken)
        {
            var aerolinea = await _aerolineaRepository.ObtenerPorIdAsync(request.Id);
            if (aerolinea == null)
            {
                return Result<bool>.Failure("Aerolínea no encontrada.");
            }

            aerolinea.Activar();
            
            await _aerolineaRepository.ActualizarAsync(aerolinea);
            
            return Result<bool>.Success(true);
        }
    }
}
