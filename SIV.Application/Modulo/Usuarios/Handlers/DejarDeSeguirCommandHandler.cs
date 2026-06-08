using MediatR;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class DejarDeSeguirCommandHandler : IRequestHandler<DejarDeSeguirCommand,bool>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public DejarDeSeguirCommandHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public async Task<bool> Handle(DejarDeSeguirCommand request, CancellationToken cancellationToken)
        {
            await _usuarioRepository.EliminarSeguimientoAsync(request.UsuarioId, request.VueloId);

            return true;
        }
    }
}

