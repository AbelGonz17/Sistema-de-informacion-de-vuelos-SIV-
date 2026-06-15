using MediatR;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Application.Modulo.Usuarios.Events;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Usuarios.Handlers.Commands
{
    public class DejarDeSeguirCommandHandler : IRequestHandler<DejarDeSeguirCommand, Result<bool>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ISeguridadService _seguridadService;
        private readonly IMediator _mediator;

        public DejarDeSeguirCommandHandler(
            IUsuarioRepository usuarioRepository,
            ISeguridadService seguridadService,
            IMediator mediator)
        {
            _usuarioRepository = usuarioRepository;
            _seguridadService = seguridadService;
            _mediator = mediator;
        }

        public async Task<Result<bool>> Handle(DejarDeSeguirCommand request, CancellationToken cancellationToken)
        {
            await _usuarioRepository.EliminarSeguimientoAsync(request.UsuarioId, request.VueloId);

            var usuarioActual = _seguridadService.ObtenerUsarioActual();

            await _mediator.Publish(new SeguimientoCesadoEvent
            {
                UsuarioId = request.UsuarioId,
                VueloId = request.VueloId,
                UsuarioActor = usuarioActual
            }, cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}

