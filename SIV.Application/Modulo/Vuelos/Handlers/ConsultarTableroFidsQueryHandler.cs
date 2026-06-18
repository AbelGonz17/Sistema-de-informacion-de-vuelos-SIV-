using MediatR;
using SIV.Application.Common.Mappings;
using SIV.Application.Common.Models;
using SIV.Application.Modulo.Vuelos.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Vuelos.Handlers
{
    public class ConsultarTableroFidsQueryHandler : IRequestHandler<ConsultarTableroFidsQuery, Result<PaginatedList<VueloTableroDto>>>
    {
        private readonly IVueloRepository _vueloRepository;

        public ConsultarTableroFidsQueryHandler(IVueloRepository vueloRepository)
        {
            _vueloRepository = vueloRepository;
        }

        public async Task<Result<PaginatedList<VueloTableroDto>>> Handle(ConsultarTableroFidsQuery request, CancellationToken cancellationToken)
        {
            var (vuelos, totalCount) = await _vueloRepository.ObtenerVuelosFidsPaginadosAsync(
                request.PageNumber,
                request.PageSize,
                request.EsLlegada,
                request.Estado,
                request.AerolineaId,
                request.Fecha
            );

            var vuelosDtoList = vuelos.Select(v => new VueloTableroDto
            {
                Id = v.Id,
                NumeroVuelo = v.NumeroVuelo,
                Aerolinea = v.AerolineaRef?.Nombre ?? v.Aerolinea.ToString(),
                Origen = v.OrigenRef?.Nombre ?? v.Origen.ToString(),
                Destino = v.DestinoRef?.Nombre ?? v.Destino.ToString(),
                Puerta = string.IsNullOrWhiteSpace(v.Puerta) ? "TBD" : v.Puerta,
                Estado = v.EstadoActual.ToString(),
                HorarioPlanificado = request.EsLlegada.HasValue && request.EsLlegada.Value ? v.HorarioPlanificadoLlegada : v.HorarioPlanificadoSalida,
                HorarioEstimado = request.EsLlegada.HasValue && request.EsLlegada.Value ? v.HorarioEstimadoLlegada : v.HorarioEstimadoSalida
            }).ToList();

            var paginatedResult = new PaginatedList<VueloTableroDto>(vuelosDtoList, totalCount, request.PageNumber, request.PageSize);

            return Result<PaginatedList<VueloTableroDto>>.Success(paginatedResult);
        }
    }
}