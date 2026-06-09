using MediatR;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Domain.Common;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Vuelos.Handlers
{
    public class CrearVueloCommandHandler : IRequestHandler<CrearVueloCommand, Result<Guid>>
    {
        private readonly IVueloRepository _vueloRepository;

        public CrearVueloCommandHandler(IVueloRepository vueloRepository)
        {
            _vueloRepository = vueloRepository;
        }

        public async Task<Result<Guid>> Handle(CrearVueloCommand request, CancellationToken cancellationToken)
        {
            var nuevoVuelo = new Vuelo(
                Guid.NewGuid(),
                request.NumeroVuelo,
                request.Aerolinea,
                request.Origen,
                request.Destino,
                request.HorarioPlanificadoSalida,
                request.HorarioPlanificadoLlegada,
                "Registro inicial del vuelo"
            );

            if (!string.IsNullOrWhiteSpace(request.Puerta))
            {
                nuevoVuelo.ActualizarPuerta(request.Puerta);
            }

            await _vueloRepository.AgregarAsync(nuevoVuelo);

            return Result<Guid>.Success(nuevoVuelo.Id);
        }
    }
}