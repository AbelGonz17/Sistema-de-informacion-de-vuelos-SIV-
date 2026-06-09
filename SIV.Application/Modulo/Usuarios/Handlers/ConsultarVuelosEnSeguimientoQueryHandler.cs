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

            var listaDtos = usuario.VuelosSeguidos.Select(vuelo => new VueloDto
            {
                Id = vuelo.Id,
                NumeroVuelo = vuelo.NumeroVuelo,
                Aerolinea = vuelo.Aerolinea,
                Origen = vuelo.Origen,
                Destino = vuelo.Destino,
                HorarioPlanificadoSalida = vuelo.HorarioPlanificadoSalida,
                HorarioEstimadoSalida = vuelo.HorarioEstimadoSalida,
                Puerta = vuelo.Puerta,
                EstadoActual = vuelo.EstadoActual.ToString()
            }).ToList();

            return Result<IEnumerable<VueloDto>>.Success(listaDtos);
        }
    }
}