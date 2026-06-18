using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Modulo.Usuarios.DTOs;
using SIV.Application.Modulo.Usuarios.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class ConsultarHistorialSeguimientosQueryHandler : IRequestHandler<ConsultarHistorialSeguimientosQuery, Result<IEnumerable<HistorialSeguimientoDto>>>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public ConsultarHistorialSeguimientosQueryHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Result<IEnumerable<HistorialSeguimientoDto>>> Handle(ConsultarHistorialSeguimientosQuery request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObtenerParaModificacionAsync(request.UsuarioId);

            if (usuario == null)
            {
                return Result<IEnumerable<HistorialSeguimientoDto>>.Failure("El usuario no existe.", StatusCodes.Status404NotFound);
            }

            var historial = usuario.Seguimientos.Select(s => new HistorialSeguimientoDto
            {
                SeguimientoId = s.Id,
                VueloId = s.VueloId,
                NumeroVuelo = s.Vuelo?.NumeroVuelo ?? "Desconocido",
                Aerolinea = s.Vuelo?.AerolineaRef?.Nombre ?? "Desconocida",
                FechaInicio = s.FechaInicio,
                FechaFin = s.FechaFin,
                Activo = s.Activo
            }).OrderByDescending(h => h.FechaInicio).ToList();

            return Result<IEnumerable<HistorialSeguimientoDto>>.Success(historial);
        }
    }
}