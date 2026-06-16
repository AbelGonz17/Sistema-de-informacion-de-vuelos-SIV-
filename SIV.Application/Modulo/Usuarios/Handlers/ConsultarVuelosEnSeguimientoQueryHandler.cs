using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Common.Mappings;
using SIV.Application.Modulo.Usuarios.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class ConsultarVuelosEnSeguimientoQueryHandler : IRequestHandler<ConsultarVuelosEnSeguimientoQuery, Result<IEnumerable<VueloDto>>>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public ConsultarVuelosEnSeguimientoQueryHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Result<IEnumerable<VueloDto>>> Handle(ConsultarVuelosEnSeguimientoQuery request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdConVuelosAsync(request.UsuarioId);

            if (usuario == null)
                return Result<IEnumerable<VueloDto>>.Failure("Usuario no encontrado", StatusCodes.Status404NotFound);

            var listaDtos = usuario.Seguimientos.Select(seguimiento => new VueloDto
            {
                Id = seguimiento.VueloId,
                NumeroVuelo = seguimiento.Vuelo.NumeroVuelo,
                Aerolinea = seguimiento.Vuelo.Aerolinea,
                Origen = seguimiento.Vuelo.Origen,
                Destino = seguimiento.Vuelo.Destino,
                HorarioPlanificadoSalida = seguimiento.Vuelo.HorarioPlanificadoSalida,
                HorarioEstimadoSalida = seguimiento.Vuelo.HorarioEstimadoSalida,
                Puerta = seguimiento.Vuelo.Puerta,
                EstadoActual = seguimiento.Vuelo.EstadoActual.ToString()
            }).ToList();

            return Result<IEnumerable<VueloDto>>.Success(listaDtos);
        }
    }
}