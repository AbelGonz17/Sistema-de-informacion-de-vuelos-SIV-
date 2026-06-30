using MediatR;
using SIV.Application.Modulo.Reportes.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;
using System.Text;

namespace SIV.Application.Modulo.Reportes.Handlers
{
    public class ExportarReportesCsvQueryHandler : IRequestHandler<ExportarReportesCsvQuery, Result<byte[]>>
    {
        private readonly IReportesRepository _reportesRepository;

        public ExportarReportesCsvQueryHandler(IReportesRepository reportesRepository)
        {
            _reportesRepository = reportesRepository;
        }

        public async Task<Result<byte[]>> Handle(ExportarReportesCsvQuery request, CancellationToken cancellationToken)
        {
            var sb = new StringBuilder();

            switch (request.TipoReporte.ToUpper())
            {
                case "OPERACION":
                    sb.AppendLine("NumeroVuelo,Origen,Destino,FechaSalida,Estado Actual");
                    var vuelos = await _reportesRepository.ObtenerVuelosPorRangoFechaAsync(
                        request.FechaInicio ?? DateTime.MinValue, 
                        request.FechaFin ?? DateTime.MaxValue);
                    foreach (var v in vuelos)
                    {
                        sb.AppendLine($"{v.NumeroVuelo},{v.OrigenRef?.Nombre ?? v.Origen.ToString()},{v.DestinoRef?.Nombre ?? v.Destino.ToString()},{v.HorarioPlanificadoSalida:yyyy-MM-dd HH:mm},{v.EstadoActual}");
                    }
                    break;

                case "CAMBIOS":
                    sb.AppendLine("NumeroVuelo,TipoCambio,FechaHora,Operador Responsable,Detalle");
                    var cambios = await _reportesRepository.ObtenerCambiosOperativosAsync(
                        request.FechaInicio ?? DateTime.MinValue, 
                        request.FechaFin ?? DateTime.MaxValue);
                    foreach (var c in cambios)
                    {
                        var detalle = c.Cambio.DetalleCambio?.Replace("\"", "\"\"") ?? "";
                        sb.AppendLine($"{c.NumeroVuelo},{c.Cambio.TipoCambio},{c.Cambio.FechaHora:yyyy-MM-dd HH:mm:ss},{c.Operador},\"{detalle}\"");
                    }
                    break;

                case "SEGUIMIENTOS":
                    sb.AppendLine("NumeroVuelo,Cantidad Seguidores Activos");
                    var seguimientos = await _reportesRepository.ObtenerTopVuelosMasSeguidosAsync(1000);
                    foreach (var s in seguimientos)
                    {
                        sb.AppendLine($"{s.NumeroVuelo},{s.CantidadSeguidores}");
                    }
                    break;

                default:
                    return Result<byte[]>.Failure("Tipo de reporte no soportado. Tipos válidos: Operacion, Cambios, Seguimientos", 400);
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return Result<byte[]>.Success(bytes);
        }
    }
}
