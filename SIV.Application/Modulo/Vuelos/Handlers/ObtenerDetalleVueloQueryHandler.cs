using MediatR;
using SIV.Application.Modulo.Vuelos.DTOs;
using SIV.Application.Modulo.Vuelos.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;
using System.Linq;

namespace SIV.Application.Modulo.Vuelos.Handlers
{
    public class ObtenerDetalleVueloQueryHandler : IRequestHandler<ObtenerDetalleVueloQuery, Result<VueloDetalleDto>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public ObtenerDetalleVueloQueryHandler(IVueloRepository vueloRepository, IUsuarioRepository usuarioRepository)
        {
            _vueloRepository = vueloRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Result<VueloDetalleDto>> Handle(ObtenerDetalleVueloQuery request, CancellationToken cancellationToken)
        {
            var vuelo = await _vueloRepository.ObtenerDetalleCompletoAsync(request.VueloId);

            if (vuelo == null)
            {
                return Result<VueloDetalleDto>.Failure("Vuelo no encontrado.", 404);
            }

            var usuariosIds = vuelo.HistorialEstados.Select(h => h.UsuarioResponsable)
                .Concat(vuelo.HistorialCambio.Select(c => c.UsuarioResponsable))
                .Distinct()
                .ToList();

            var usuariosDict = await _usuarioRepository.ObtenerNombresPorIdsAsync(usuariosIds);

            var dto = new VueloDetalleDto(
                vuelo.Id,
                vuelo.NumeroVuelo,
                vuelo.AerolineaRef?.Nombre ?? vuelo.Aerolinea.ToString(),
                vuelo.OrigenRef?.Nombre ?? vuelo.Origen.ToString(),
                vuelo.DestinoRef?.Nombre ?? vuelo.Destino.ToString(),
                vuelo.HorarioPlanificadoSalida,
                vuelo.HorarioPlanificadoLlegada,
                vuelo.HorarioEstimadoSalida,
                vuelo.HorarioEstimadoLlegada,
                vuelo.Puerta,
                vuelo.EstadoActual.ToString(),
                vuelo.MotivoUltimoCambio,
                vuelo.HistorialEstados.Select(h => new HistorialEstadoDto
                {
                    EstadoAnterior = h.EstadoAnterior.ToString(),
                    EstadoNuevo = h.EstadoNuevo.ToString(),
                    FechaHora = h.FechaHora,
                    UsuarioResponsable = h.UsuarioResponsable == Guid.Empty ? "Sistema" : usuariosDict.GetValueOrDefault(h.UsuarioResponsable, "Desconocido")
                }).ToList(),
                vuelo.HistorialCambio.Select(c => new HistorialCambioOperativoDto
                {
                    TipoCambio = c.TipoCambio,
                    Motivo = c.Motivo,
                    DetalleCambio = c.DetalleCambio,
                    FechaHora = c.FechaHora,
                    UsuarioResponsable = c.UsuarioResponsable == Guid.Empty ? "Sistema" : usuariosDict.GetValueOrDefault(c.UsuarioResponsable, "Desconocido")
                }).ToList()
            );

            return Result<VueloDetalleDto>.Success(dto);
        }
    }
}
