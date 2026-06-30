using MediatR;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class CerrarSesionCommandHandler : IRequestHandler<CerrarSesionCommand, Result<bool>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CerrarSesionCommandHandler(IUsuarioRepository usuarioRepository, IUnitOfWork unitOfWork)
        {
            _usuarioRepository = usuarioRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(CerrarSesionCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObtenerParaModificacionAsync(request.UsuarioId);

            if (usuario == null)
            {
                return Result<bool>.Failure("Usuario no encontrado.", 404);
            }

            if (!string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                usuario.RevocarRefreshToken(request.RefreshToken);
            }
            else
            {
                usuario.RevocarTodosRefreshTokens();
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
