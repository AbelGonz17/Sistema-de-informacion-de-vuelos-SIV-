using MediatR;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class IniciarSeguimientoCommandHandler : IRequestHandler<IniciarSeguimientoCommand,bool>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public IniciarSeguimientoCommandHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<bool> Handle(IniciarSeguimientoCommand request, CancellationToken cancellationToken)
        {
            await _usuarioRepository.RegistrarSeguimientoAsync(request.UsuarioId, request.VueloId);

            return true;
        }
    }
}