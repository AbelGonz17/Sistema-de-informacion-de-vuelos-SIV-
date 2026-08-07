using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Modulo.Vuelos.DTOs;
using SIV.Application.Modulo.Vuelos.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Vuelos.Handlers
{
    public class ObtenerHistorialVueloQueryHandler : IRequestHandler<ObtenerHistorialVueloQuery, Result<HistorialVueloDto>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public ObtenerHistorialVueloQueryHandler(IVueloRepository vueloRepository, IUsuarioRepository usuarioRepository)
        {
            _vueloRepository = vueloRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Result<HistorialVueloDto>> Handle(ObtenerHistorialVueloQuery request, CancellationToken cancellationToken)
        {
            var vuelo = await _vueloRepository.ObtenerPorIdConHistorialAsync(request.VueloId);

            if (vuelo == null)
            {
                return Result<HistorialVueloDto>.Failure("El vuelo especificado no existe.", StatusCodes.Status404NotFound);
            }

            var usuariosIds = vuelo.HistorialEstados.Select(he => he.UsuarioResponsable)
                .Concat(vuelo.HistorialCambio.Select(hc => hc.UsuarioResponsable))
                .Distinct()
                .ToList();

            var usuariosDict = await _usuarioRepository.ObtenerNombresPorIdsAsync(usuariosIds);

            var dto = new HistorialVueloDto
            {
                VueloId = vuelo.Id,
                NumeroVuelo = vuelo.NumeroVuelo,
                HistorialEstados = vuelo.HistorialEstados.Select(he => new HistorialEstadoDto
                {
                    EstadoAnterior = he.EstadoAnterior.ToString(),
                    EstadoNuevo = he.EstadoNuevo.ToString(),
                    FechaHora = he.FechaHora,
                    UsuarioResponsable = he.UsuarioResponsable == Guid.Empty ? "Sistema" : usuariosDict.GetValueOrDefault(he.UsuarioResponsable, "Desconocido")
                }).OrderByDescending(h => h.FechaHora).ToList(),
                HistorialCambios = vuelo.HistorialCambio.Select(hc => new HistorialCambioOperativoDto
                {
                    TipoCambio = hc.TipoCambio,
                    Motivo = hc.Motivo,
                    DetalleCambio = hc.DetalleCambio,
                    FechaHora = hc.FechaHora,
                    UsuarioResponsable = hc.UsuarioResponsable == Guid.Empty ? "Sistema" : usuariosDict.GetValueOrDefault(hc.UsuarioResponsable, "Desconocido")
                }).OrderByDescending(h => h.FechaHora).ToList()
            };

            return Result<HistorialVueloDto>.Success(dto);
        }
    }
}