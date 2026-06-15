using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Application.Modulo.Usuarios.Events;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Usuarios.Handlers.Commands
{
    public class CambiarEstadoUsuarioCommandHandler : IRequestHandler<CambiarEstadoUsuarioCommand, Result<bool>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ISeguridadService _seguridadService;
        private readonly IMediator _mediator;

        public CambiarEstadoUsuarioCommandHandler(
            IUsuarioRepository usuarioRepository,
            ISeguridadService seguridadService,
            IMediator mediator)
        {
            _usuarioRepository = usuarioRepository;
            _seguridadService = seguridadService;
            _mediator = mediator;
        }

        public async Task<Result<bool>> Handle(CambiarEstadoUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(request.UsuarioId);
            if (usuario == null)           
                return Result<bool>.Failure("El usuario que intenta gestionar no existe.", StatusCodes.Status404NotFound);
            
            usuario.CambiarEstadoActivo(request.Activo);
            await _usuarioRepository.ActualizarAsync(usuario);

            var administradorActual = _seguridadService.ObtenerUsarioActual();

            await _mediator.Publish(new UsuarioEstadoCambiadoEvent
            {
                UsuarioId = usuario.Id,
                NuevoEstado = request.Activo,
                UsuarioActor = administradorActual
            }, cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}