using MediatR;
using SIV.Application.Common.Models;
using SIV.Application.Modulo.Auditoria.DTOs;
using SIV.Application.Modulo.Auditoria.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Auditoria.Handlers
{
    public class ConsultarLogAuditoriaQueryHandler : IRequestHandler<ConsultarLogAuditoriaQuery, Result<PaginatedList<LogAuditoriaDto>>>
    {
        private readonly IAuditoriaRepository _auditoriaRepository;

        public ConsultarLogAuditoriaQueryHandler(IAuditoriaRepository auditoriaRepository)
        {
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task<Result<PaginatedList<LogAuditoriaDto>>> Handle(ConsultarLogAuditoriaQuery request, CancellationToken cancellationToken)
        {
            var (logs, totalCount) = await _auditoriaRepository.ObtenerLogsPaginadosAsync(
                request.PageNumber,
                request.PageSize,
                request.FechaInicio,
                request.FechaFin,
                request.Accion,
                request.Busqueda
            );

            var logsDto = logs.Select(l => 
            {
                string entidadAfectada = "";
                string entidadId = "";

                if (!string.IsNullOrEmpty(l.Detalles))
                {
                    string detallesTrimmed = l.Detalles.TrimStart();
                    if (detallesTrimmed.StartsWith("{"))
                    {
                        try
                        {
                            using var doc = System.Text.Json.JsonDocument.Parse(l.Detalles);
                            var root = doc.RootElement;
                            if (root.TryGetProperty("Entidad", out var entidadProp))
                            {
                                entidadAfectada = entidadProp.GetString() ?? "";
                            }
                            if (root.TryGetProperty("EntidadId", out var idProp))
                            {
                                entidadId = idProp.GetString() ?? "";
                            }
                        }
                        catch
                        {
                            // Ignorar
                        }
                    }
                }

                // Si no se obtuvo de la estructura JSON del DbContext, deducirla de la Acción y del Payload del Command
                if (string.IsNullOrEmpty(entidadAfectada))
                {
                    var lowerAccion = l.Accion.ToLower();
                    if (lowerAccion.Contains("usuario")) entidadAfectada = "Usuario";
                    else if (lowerAccion.Contains("aeropuerto")) entidadAfectada = "Aeropuerto";
                    else if (lowerAccion.Contains("aerolinea")) entidadAfectada = "Aerolínea";
                    else if (lowerAccion.Contains("vuelo") || lowerAccion.Contains("retraso") || lowerAccion.Contains("adelanto") || lowerAccion.Contains("puerta")) entidadAfectada = "Vuelo";
                    else entidadAfectada = "N/A";
                }

                if (string.IsNullOrEmpty(entidadId))
                {
                    entidadId = extraerEntidadId(l.Detalles);
                }

                if (string.IsNullOrEmpty(entidadAfectada)) entidadAfectada = "N/A";
                if (string.IsNullOrEmpty(entidadId)) entidadId = "N/A";

                return new LogAuditoriaDto
                {
                    Id = l.Id,
                    FechaHora = l.FechaRegistro,
                    Usuario = l.Usuario ?? "Sistema",
                    Accion = l.Accion,
                    Detalles = l.Detalles,
                    EntidadAfectada = entidadAfectada,
                    EntidadId = entidadId
                };
            }).ToList();

            var paginatedResult = new PaginatedList<LogAuditoriaDto>(logsDto, totalCount, request.PageNumber, request.PageSize);

            return Result<PaginatedList<LogAuditoriaDto>>.Success(paginatedResult);
        }

        private static string extraerEntidadId(string detalles)
        {
            if (string.IsNullOrEmpty(detalles)) return "";
            int payloadIdx = detalles.IndexOf("Payload: ");
            if (payloadIdx == -1) return "";
            
            string payloadJson = detalles.Substring(payloadIdx + 9).Trim();
            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(payloadJson);
                var root = doc.RootElement;
                if (root.TryGetProperty("Id", out var idProp)) return idProp.ToString();
                if (root.TryGetProperty("id", out var idPropLower)) return idPropLower.ToString();
                if (root.TryGetProperty("UsuarioId", out var uIdProp)) return uIdProp.ToString();
                if (root.TryGetProperty("usuarioId", out var uIdPropLower)) return uIdPropLower.ToString();
                if (root.TryGetProperty("VueloId", out var vIdProp)) return vIdProp.ToString();
                if (root.TryGetProperty("vueloId", out var vIdPropLower)) return vIdPropLower.ToString();
                if (root.TryGetProperty("AerolineaId", out var alIdProp)) return alIdProp.ToString();
                if (root.TryGetProperty("aerolineaId", out var alIdPropLower)) return alIdPropLower.ToString();
                if (root.TryGetProperty("AeropuertoId", out var apIdProp)) return apIdProp.ToString();
                if (root.TryGetProperty("aeropuertoId", out var apIdPropLower)) return apIdPropLower.ToString();
            }
            catch
            {
                // Ignorar
            }
            return "";
        }
    }
}