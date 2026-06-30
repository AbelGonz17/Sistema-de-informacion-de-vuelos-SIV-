using MediatR;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Vuelos.Handlers
{
    public class ActualizarDatosBasicosVueloCommandHandler : IRequestHandler<ActualizarDatosBasicosVueloCommand, Result<Guid>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ActualizarDatosBasicosVueloCommandHandler(IVueloRepository vueloRepository, IUnitOfWork unitOfWork)
        {
            _vueloRepository = vueloRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(ActualizarDatosBasicosVueloCommand request, CancellationToken cancellationToken)
        {
            var vuelo = await _vueloRepository.ObtenerPorIdAsync(request.VueloId);

            if (vuelo == null)
            {
                return Result<Guid>.Failure("Vuelo no encontrado.", 404);
            }

            try
            {
                vuelo.ActualizarDatosBasicos(
                    request.Aerolinea,
                    request.Origen,
                    request.Destino,
                    request.HorarioPlanificadoSalida,
                    request.HorarioPlanificadoLlegada,
                    request.Puerta,
                    request.UsuarioId
                );

                await _vueloRepository.ActualizarAsync(vuelo);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<Guid>.Success(vuelo.Id);
            }
            catch (InvalidOperationException ex)
            {
                return Result<Guid>.Failure(ex.Message, 400);
            }
        }
    }
}
