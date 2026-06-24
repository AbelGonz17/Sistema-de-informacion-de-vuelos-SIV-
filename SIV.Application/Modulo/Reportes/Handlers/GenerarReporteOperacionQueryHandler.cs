using MediatR;
using SIV.Application.Modulo.Reportes.DTOs;
using SIV.Application.Modulo.Reportes.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Reportes.Handlers
{
    public class GenerarReporteOperacionQueryHandler : IRequestHandler<GenerarReporteOperacionQuery, Result<ReporteOperacionDto>>
    {
        private readonly IReportesRepository _reportesRepository;

        public GenerarReporteOperacionQueryHandler(IReportesRepository reportesRepository)
        {
            _reportesRepository = reportesRepository;
        }

        public async Task<Result<ReporteOperacionDto>> Handle(GenerarReporteOperacionQuery request, CancellationToken cancellationToken)
        {
            if (request.FechaInicio > request.FechaFin)
            {
                return Result<ReporteOperacionDto>.Failure("La fecha de inicio no puede ser mayor que la fecha de fin.", 400);
            }

            var vuelos = await _reportesRepository.ObtenerVuelosPorRangoFechaAsync(request.FechaInicio, request.FechaFin);

            var reporte = new ReporteOperacionDto
            {
                TotalVuelos = vuelos.Count(),
                Completados = vuelos.Count(v => v.EstadoActual == EstadoVuelo.Completado),
                Cancelados = vuelos.Count(v => v.EstadoActual == EstadoVuelo.Cancelado),
                Retrasados = vuelos.Count(v => v.EstadoActual == EstadoVuelo.Retrasado),
                Otros = vuelos.Count(v => v.EstadoActual != EstadoVuelo.Completado && v.EstadoActual != EstadoVuelo.Cancelado && v.EstadoActual != EstadoVuelo.Retrasado),
                Vuelos = vuelos.Select(v => new VueloOperacionDetalleDto
                {
                    VueloId = v.Id,
                    NumeroVuelo = v.NumeroVuelo,
                    Aerolinea = v.AerolineaRef?.Nombre ?? "Desconocida",
                    Origen = v.OrigenRef?.Nombre ?? "Desconocido",
                    Destino = v.DestinoRef?.Nombre ?? "Desconocido",
                    HorarioPlanificadoSalida = v.HorarioPlanificadoSalida,
                    EstadoActual = v.EstadoActual.ToString()
                }).ToList()
            };

            return Result<ReporteOperacionDto>.Success(reporte);
        }
    }
}
