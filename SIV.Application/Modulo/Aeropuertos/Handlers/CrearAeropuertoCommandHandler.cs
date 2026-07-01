using MediatR;
using SIV.Application.Modulo.Aeropuertos.Commands;
using SIV.Domain.Common;
using SIV.Domain.Entities.Catalogo;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Aeropuertos.Handlers
{
    public class CrearAeropuertoCommandHandler : IRequestHandler<CrearAeropuertoCommand, Result<Guid>>
    {
        private readonly IAeropuertoRepository _aeropuertoRepository;

        public CrearAeropuertoCommandHandler(IAeropuertoRepository aeropuertoRepository)
        {
            _aeropuertoRepository = aeropuertoRepository;
        }

        public async Task<Result<Guid>> Handle(CrearAeropuertoCommand request, CancellationToken cancellationToken)
        {
            var todos = await _aeropuertoRepository.ObtenerTodosAsync();
            if (todos.Any(a => a.Codigo.Equals(request.Codigo, StringComparison.OrdinalIgnoreCase)))
            {
                return Result<Guid>.Failure($"Ya existe un aeropuerto con el código {request.Codigo}");
            }

            var aeropuerto = new Aeropuerto
            {
                Id = Guid.NewGuid(),
                Codigo = request.Codigo,
                Nombre = request.Nombre,
                Pais = request.Pais
            };

            await _aeropuertoRepository.AgregarAsync(aeropuerto);

            return Result<Guid>.Success(aeropuerto.Id);
        }
    }
}