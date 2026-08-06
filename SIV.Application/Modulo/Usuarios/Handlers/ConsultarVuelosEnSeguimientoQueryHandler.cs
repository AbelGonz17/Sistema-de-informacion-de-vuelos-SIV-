using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Modulo.Usuarios.DTOs;
using SIV.Application.Modulo.Usuarios.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class ConsultarVuelosEnSeguimientoQueryHandler : IRequestHandler<ConsultarVuelosEnSeguimientoQuery, Result<IEnumerable<HistorialSeguimientoDto>>>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public ConsultarVuelosEnSeguimientoQueryHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Result<IEnumerable<HistorialSeguimientoDto>>> Handle(ConsultarVuelosEnSeguimientoQuery request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdConVuelosAsync(request.UsuarioId);

            if (usuario == null)
                return Result<IEnumerable<HistorialSeguimientoDto>>.Failure("Usuario no encontrado", StatusCodes.Status404NotFound);

            var listaDtos = usuario.Seguimientos.Select(seguimiento => new HistorialSeguimientoDto
            {
                SeguimientoId = seguimiento.Id,
                VueloId = seguimiento.VueloId,
                NumeroVuelo = seguimiento.Vuelo.NumeroVuelo,
                Aerolinea = seguimiento.Vuelo.AerolineaRef?.Nombre ?? "",
                FechaInicio = seguimiento.FechaInicio,
                FechaFin = seguimiento.FechaFin,
                Activo = seguimiento.Activo,
                Origen = seguimiento.Vuelo.OrigenRef?.Nombre ?? "N/A",
                Destino = seguimiento.Vuelo.DestinoRef?.Nombre ?? "N/A",
                EstadoActual = seguimiento.Vuelo.EstadoActual.ToString(),
                HorarioPlanificado = seguimiento.Vuelo.HorarioPlanificadoSalida,
                HorarioEstimado = seguimiento.Vuelo.HorarioEstimadoSalida,
                Puerta = seguimiento.Vuelo.Puerta ?? "N/A"
            }).ToList();

            return Result<IEnumerable<HistorialSeguimientoDto>>.Success(listaDtos);
        }
    }
}