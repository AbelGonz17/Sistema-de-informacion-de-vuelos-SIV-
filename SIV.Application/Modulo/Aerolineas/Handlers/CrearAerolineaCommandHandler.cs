using MediatR;
using SIV.Application.Modulo.Aerolineas.Commands;
using SIV.Domain.Common;
using SIV.Domain.Entities.Catalogo;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Aerolineas.Handlers
{
    public class CrearAerolineaCommandHandler : IRequestHandler<CrearAerolineaCommand, Result<Guid>>
    {
        private readonly IAerolineaRepository _aerolineaRepository;

        public CrearAerolineaCommandHandler(IAerolineaRepository aerolineaRepository)
        {
            _aerolineaRepository = aerolineaRepository;
        }

        public async Task<Result<Guid>> Handle(CrearAerolineaCommand request, CancellationToken cancellationToken)
        {
            var todas = await _aerolineaRepository.ObtenerTodasAsync();
            if (todas.Any(a => a.Codigo.Equals(request.Codigo, StringComparison.OrdinalIgnoreCase)))
            {
                return Result<Guid>.Failure($"Ya existe una aerolínea con el código {request.Codigo}");
            }

            var aerolinea = new Aerolinea
            {
                Id = Guid.NewGuid(),
                Codigo = request.Codigo,
                Nombre = request.Nombre
            };

            await _aerolineaRepository.AgregarAsync(aerolinea);

            return Result<Guid>.Success(aerolinea.Id);
        }
    }
}