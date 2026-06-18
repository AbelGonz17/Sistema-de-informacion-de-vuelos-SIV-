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

        public DejarDeSeguirCommandHandler(IUsuarioRepository usuarioRepository, IVueloRepository vueloRepository)
        {
            _usuarioRepository = usuarioRepository;
            _vueloRepository = vueloRepository;
        }

        public async Task<Result<bool>> Handle(DejarDeSeguirCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObtenerParaModificacionAsync(request.UsuarioId);
            if (usuario == null) return Result<bool>.Failure("El usuario no existe");

            var vuelo = await _vueloRepository.ObtenerPorIdAsync(request.VueloId);
            if (vuelo == null) return Result<bool>.Failure("El vuelo no existe");

            usuario.DejarDeSeguir(vuelo);
            await _usuarioRepository.ActualizarAsync(usuario);

            return Result<bool>.Success(true);
        }
    }
}