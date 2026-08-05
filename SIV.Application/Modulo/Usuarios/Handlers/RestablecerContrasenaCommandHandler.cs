using MediatR;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class RestablecerContrasenaCommandHandler : IRequestHandler<RestablecerContrasenaCommand, Result<bool>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;

        public RestablecerContrasenaCommandHandler(IUsuarioRepository usuarioRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(RestablecerContrasenaCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObtenerPorCorreoAsync(request.CorreoElectronico);
            if (usuario == null)
            {
                return Result<bool>.Failure("Solicitud de recuperación inválida.");
            }

            if (!usuario.EsTokenRecuperacionValido(request.Token))
            {
                return Result<bool>.Failure("El enlace de recuperación es inválido o ha expirado.");
            }

            var nuevoHash = _passwordHasher.Hash(request.NuevaContrasena);
            usuario.CambiarContrasena(nuevoHash);
            usuario.LimpiarTokenRecuperacion();

            await _usuarioRepository.ActualizarAsync(usuario);
            await _unitOfWork.CommitAsync();

            return Result<bool>.Success(true);
        }
    }
}
