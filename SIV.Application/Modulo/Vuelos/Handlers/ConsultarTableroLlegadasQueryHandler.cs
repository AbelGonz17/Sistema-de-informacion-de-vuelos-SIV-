using MediatR;
using SIV.Application.Common.Mappings;
using SIV.Application.Modulo.Vuelos.Queries;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Vuelos.Handlers
{
    public class ConsultarTableroLlegadasQueryHandler : IRequestHandler<ConsultarTableroLlegadasQuery, IEnumerable<VueloDto>>
    {
        private readonly IVueloRepository _vueloRepository;

        public ConsultarTableroLlegadasQueryHandler(IVueloRepository vueloRepository)
        {
            _vueloRepository = vueloRepository;
        }

        public async Task<IEnumerable<VueloDto>> Handle(ConsultarTableroLlegadasQuery request, CancellationToken cancellationToken)
        {
            var vuelos = await _vueloRepository.ObtenerVuelosPorFechaYTipoAsync(request.Fecha, request.EsLlegada);

            return vuelos.Select(v => new VueloDto
            {
                Id = v.Id,
                NumeroVuelo = v.NumeroVuelo,
                Aerolinea = v.Aerolinea,
                Origen = v.Origen,
                Destino = v.Destino,
                HorarioPlanificadoSalida = v.HorarioPlanificadoSalida,
                HorarioEstimadoSalida = v.HorarioEstimadoSalida,
                Puerta = v.Puerta,
                EstadoActual = v.EstadoActual.ToString()
            });
        }
    }
}