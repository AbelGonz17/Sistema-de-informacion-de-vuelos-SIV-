using MediatR;
using SIV.Application.Common.Mappings;
using SIV.Application.Modulo.Vuelos.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Vuelos.Handlers
{
    public class ConsultarTableroLlegadasQueryHandler : IRequestHandler<ConsultarTableroLlegadasQuery, Result<IEnumerable<VueloTableroDto>>>
    {
        private readonly IVueloRepository _vueloRepository;

        public ConsultarTableroLlegadasQueryHandler(IVueloRepository vueloRepository)
        {
            _vueloRepository = vueloRepository;
        }

        public async Task<Result<IEnumerable<VueloTableroDto>>> Handle(ConsultarTableroLlegadasQuery request, CancellationToken cancellationToken)
        {
            var vuelosDb = await _vueloRepository.ObtenerVuelosPorFechaYTipoAsync(request.Fecha, request.EsLlegada);

            var listadoTablero = vuelosDb.Select(v => new VueloTableroDto
            {
                Id = v.Id,
                NumeroVuelo = v.NumeroVuelo,
                Aerolinea = v.Aerolinea,
                Origen = v.Origen,
                Destino = v.Destino,
                Puerta = string.IsNullOrWhiteSpace(v.Puerta) ? "TBD" : v.Puerta,
                Estado = v.EstadoActual.ToString(),

                HorarioPlanificado = request.EsLlegada ? v.HorarioPlanificadoLlegada : v.HorarioPlanificadoSalida,
                HorarioEstimado = request.EsLlegada ? v.HorarioEstimadoLlegada : v.HorarioEstimadoSalida 
            }).OrderBy(v => v.HorarioPlanificado); 

            return Result<IEnumerable<VueloTableroDto>>.Success(listadoTablero);
        }
    }
}