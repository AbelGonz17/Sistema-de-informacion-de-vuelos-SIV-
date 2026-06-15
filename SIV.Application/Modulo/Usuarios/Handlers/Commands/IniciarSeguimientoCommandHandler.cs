using MediatR;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Application.Modulo.Usuarios.Events;
using SIV.Domain.Common;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class IniciarSeguimientoCommandHandler : IRequestHandler<IniciarSeguimientoCommand, Result<bool>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ISeguridadService _seguridadService;
        private readonly IMediator _mediator;

        public IniciarSeguimientoCommandHandler(
            IUsuarioRepository usuarioRepository,
            ISeguridadService seguridadService,
            IMediator mediator)
        {
            _usuarioRepository = usuarioRepository;
            _seguridadService = seguridadService;
            _mediator = mediator;
        }

        public async Task<Result<bool>> Handle(IniciarSeguimientoCommand request, CancellationToken cancellationToken)
        {
            await _usuarioRepository.RegistrarSeguimientoAsync(request.UsuarioId, request.VueloId);

            var usuarioActual = _seguridadService.ObtenerUsarioActual();

            await _mediator.Publish(new SeguimientoIniciadoEvent
            {
                UsuarioId = request.UsuarioId,
                VueloId = request.VueloId,
                UsuarioActor = usuarioActual
            }, cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}