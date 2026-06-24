using MediatR;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class DejarDeSeguirCommandHandler : IRequestHandler<DejarDeSeguirCommand, Result<bool>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IVueloRepository _vueloRepository;
        private readonly IMediator _mediator;

        public DejarDeSeguirCommandHandler(IUsuarioRepository usuarioRepository, IVueloRepository vueloRepository, IMediator mediator)
        {
            _usuarioRepository = usuarioRepository;
            _vueloRepository = vueloRepository;
            _mediator = mediator;
        }

        public async Task<Result<bool>> Handle(DejarDeSeguirCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObtenerParaModificacionAsync(request.UsuarioId);
            if (usuario == null) return Result<bool>.Failure("El usuario no existe");

            var vuelo = await _vueloRepository.ObtenerPorIdAsync(request.VueloId);
            if (vuelo == null) return Result<bool>.Failure("El vuelo no existe");

            usuario.DejarDeSeguir(vuelo);
            await _usuarioRepository.ActualizarAsync(usuario);

            await _mediator.Publish(new UsuarioSigueVueloEvent
            {
                CorreoUsuario = usuario.Correo,
                NumeroVuelo = vuelo.NumeroVuelo,
                Accion = "DejarDeSeguir"
            }, cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}