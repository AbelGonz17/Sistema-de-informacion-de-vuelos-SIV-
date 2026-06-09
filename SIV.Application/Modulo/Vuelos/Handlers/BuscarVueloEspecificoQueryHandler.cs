using MediatR;
using SIV.Application.Common.Mappings;
using SIV.Application.Modulo.Vuelos.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Vuelos.Handlers
{
    public class BuscarVueloEspecificoQueryHandler : IRequestHandler<BuscarVueloEspecificoQuery, Result<VueloDto>>
    {
        private readonly IVueloRepository _vueloRepository;

        public BuscarVueloEspecificoQueryHandler(IVueloRepository vueloRepository)
        {
            _vueloRepository = vueloRepository;
        }

        public async Task<Result<VueloDto>> Handle(BuscarVueloEspecificoQuery request, CancellationToken cancellationToken)
        {
            var vuelo = await _vueloRepository.ObtenerPorNumeroAsync(request.NumeroVuelo);
            if (vuelo == null) 
                return Result<VueloDto>.Failure("Vuelo no encontrado");

            return Result<VueloDto>.Success(new VueloDto
            {
                Id = vuelo.Id,
                NumeroVuelo = vuelo.NumeroVuelo,
                Aerolinea = vuelo.Aerolinea,
                Origen = vuelo.Origen,
                Destino = vuelo.Destino,
                HorarioPlanificadoSalida = vuelo.HorarioPlanificadoSalida,
                HorarioEstimadoSalida = vuelo.HorarioEstimadoSalida,
                Puerta = vuelo.Puerta,
                EstadoActual = vuelo.EstadoActual.ToString()
            });
        }
    }
}