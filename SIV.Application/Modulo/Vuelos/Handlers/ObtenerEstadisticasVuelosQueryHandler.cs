using MediatR;
using SIV.Application.Modulo.Vuelos.DTOs;
using SIV.Application.Modulo.Vuelos.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Modulo.Vuelos.Handlers
{
    public class ObtenerEstadisticasVuelosQueryHandler : IRequestHandler<ObtenerEstadisticasVuelosQuery, Result<EstadisticasVuelosDto>>
    {
        private readonly IVueloRepository _vueloRepository;

        public ObtenerEstadisticasVuelosQueryHandler(IVueloRepository vueloRepository)
        {
            _vueloRepository = vueloRepository;
        }

        public async Task<Result<EstadisticasVuelosDto>> Handle(ObtenerEstadisticasVuelosQuery request, CancellationToken cancellationToken)
        {
            var vuelos = await _vueloRepository.ObtenerTodosAsync();
            var totalVuelos = vuelos.Count();

            if (totalVuelos == 0)
            {
                return Result<EstadisticasVuelosDto>.Success(new EstadisticasVuelosDto());
            }

            var vuelosPorEstado = vuelos
                .GroupBy(v => v.EstadoActual.ToString())
                .ToDictionary(g => g.Key, g => g.Count());

            var retrasados = vuelos.Count(v => v.EstadoActual == EstadoVuelo.Retrasado);
            var cancelados = vuelos.Count(v => v.EstadoActual == EstadoVuelo.Cancelado);

            var estadisticas = new EstadisticasVuelosDto
            {
                TotalVuelos = totalVuelos,
                PorcentajeRetrasos = (double)retrasados / totalVuelos * 100,
                PorcentajeCancelaciones = (double)cancelados / totalVuelos * 100,
                VuelosPorEstado = vuelosPorEstado
            };

            return Result<EstadisticasVuelosDto>.Success(estadisticas);
        }
    }
}
