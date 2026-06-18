using MediatR;
using SIV.Application.Modulo.Aeropuertos.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Aeropuertos.Handlers
{
    public class ActualizarAeropuertoCommandHandler : IRequestHandler<ActualizarAeropuertoCommand, Result<bool>>
    {
        private readonly IAeropuertoRepository _aeropuertoRepository;

        public ActualizarAeropuertoCommandHandler(IAeropuertoRepository aeropuertoRepository)
        {
            _aeropuertoRepository = aeropuertoRepository;
        }

        public async Task<Result<bool>> Handle(ActualizarAeropuertoCommand request, CancellationToken cancellationToken)
        {
            var aeropuerto = await _aeropuertoRepository.ObtenerPorIdAsync(request.Id);
            if (aeropuerto == null)
                return Result<bool>.Failure($"No se encontró el aeropuerto con Id {request.Id}");
            

            bool codigoDuplicado = await _aeropuertoRepository.ExisteCodigoParaOtroAeropuertoAsync(request.Id, request.Codigo);
            if (codigoDuplicado)    
                return Result<bool>.Failure($"Ya existe otro aeropuerto registrado con el código {request.Codigo}.");
            
            aeropuerto.Codigo = request.Codigo;
            aeropuerto.Nombre = request.Nombre;
            aeropuerto.Pais = request.Pais;

            await _aeropuertoRepository.ActualizarAsync(aeropuerto);

            return Result<bool>.Success(true);
        }
    }
}