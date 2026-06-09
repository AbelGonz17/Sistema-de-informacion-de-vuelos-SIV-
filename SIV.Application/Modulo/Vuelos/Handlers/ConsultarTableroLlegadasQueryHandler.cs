using MediatR;
using SIV.Application.Common.Mappings;
using SIV.Application.Modulo.Vuelos.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Vuelos.Handlers
{
    public class ConsultarTableroLlegadasQueryHandler : IRequestHandler<ConsultarTableroLlegadasQuery, Result<IEnumerable<VueloDto>>>
    {
        private readonly IVueloRepository _vueloRepository;

        public ConsultarTableroLlegadasQueryHandler(IVueloRepository vueloRepository)
        {
            _vueloRepository = vueloRepository;
        }

        public async Task<Result<IEnumerable<VueloDto>>> Handle(ConsultarTableroLlegadasQuery request, CancellationToken cancellationToken)
        {
            var vuelos = await _vueloRepository.ObtenerVuelosPorFechaYTipoAsync(request.Fecha, request.EsLlegada);

            if (vuelos == null || !vuelos.Any())
            {
                string tipoTablero = request.EsLlegada ? "llegadas" : "salidas";
                return Result<IEnumerable<VueloDto>>.Failure(
                    $"No se encontraron operaciones de {tipoTablero} registradas para la fecha {request.Fecha:dd/MM/yyyy}.",
                    Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound
                );
            }

            var listaDtos = vuelos.Select(v => new VueloDto
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
            }).ToList();

            return Result<IEnumerable<VueloDto>>.Success(listaDtos);
        }
    }
}