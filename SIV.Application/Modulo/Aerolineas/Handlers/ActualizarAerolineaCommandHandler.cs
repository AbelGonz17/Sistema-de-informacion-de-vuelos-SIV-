using MediatR;
using SIV.Application.Modulo.Aerolineas.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Aerolineas.Handlers
{
    public class ActualizarAerolineaCommandHandler : IRequestHandler<ActualizarAerolineaCommand, Result<bool>>
    {
        private readonly IAerolineaRepository _aerolineaRepository;

        public ActualizarAerolineaCommandHandler(IAerolineaRepository aerolineaRepository)
        {
            _aerolineaRepository = aerolineaRepository;
        }

        public async Task<Result<bool>> Handle(ActualizarAerolineaCommand request, CancellationToken cancellationToken)
        {
            var aerolinea = await _aerolineaRepository.ObtenerPorIdAsync(request.Id);
            if (aerolinea == null)
            {
                return Result<bool>.Failure($"No se encontró la aerolínea con Id {request.Id}");
            }

            bool codigoDuplicado = await _aerolineaRepository.ExisteCodigoParaOtraAerolineaAsync(request.Id, request.Codigo);
            if (codigoDuplicado)
            {
                return Result<bool>.Failure($"Ya existe otra aerolínea registrada con el código {request.Codigo}.");
            }

            aerolinea.Codigo = request.Codigo;
            aerolinea.Nombre = request.Nombre;

            await _aerolineaRepository.ActualizarAsync(aerolinea);

            return Result<bool>.Success(true);
        }
    }
}