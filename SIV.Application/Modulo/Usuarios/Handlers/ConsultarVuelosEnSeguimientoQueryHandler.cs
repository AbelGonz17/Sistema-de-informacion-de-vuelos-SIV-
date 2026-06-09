using MediatR;
using SIV.Application.Common.Mappings;
using SIV.Application.Modulo.Usuarios.Queries;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class ConsultarVuelosEnSeguimientoQueryHandler : IRequestHandler<ConsultarVuelosEnSeguimientoQuery, IEnumerable<VueloDto>>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public ConsultarVuelosEnSeguimientoQueryHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<VueloDto>> Handle(ConsultarVuelosEnSeguimientoQuery request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdConVuelosAsync(request.UsuarioId);

            if (usuario == null)
                return new List<VueloDto>();
            
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

            return listaDtos;
        }
    }
}