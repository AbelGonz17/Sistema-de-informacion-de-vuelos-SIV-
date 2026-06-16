using MediatR;
using SIV.Application.Modulo.Aeropuertos.Commands;
using SIV.Domain.Common;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            // Validar si el aeropuerto ya existe
            var todos = await _aeropuertoRepository.ObtenerTodosAsync();
            if (todos.Any(a => a.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return Result<Guid>.Failure($"Ya existe un aeropuerto con el nombre {request.Name}");
            }

            var aeropuerto = new Aeropuerto
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Pais = request.Pais
            };

            await _aeropuertoRepository.AgregarAsync(aeropuerto);

            return Result<Guid>.Success(aeropuerto.Id);
        }
    }
}
