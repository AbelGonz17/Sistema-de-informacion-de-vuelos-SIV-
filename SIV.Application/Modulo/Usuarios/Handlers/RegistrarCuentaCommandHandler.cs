using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;


namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class RegistrarCuentaCommandHandler : IRequestHandler<RegistrarCuentaCommand, Result<string>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;

        public RegistrarCuentaCommandHandler(IUsuarioRepository usuarioRepository, ITokenService tokenService, IPasswordHasher passwordHasher)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<string>> Handle(RegistrarCuentaCommand request, CancellationToken cancellationToken)
        {
            var usuarioExistente =
                await _usuarioRepository.ObtenerPorCorreoAsync(request.Correo);

            if (usuarioExistente != null)
                return Result<string>.Failure("El correo electrónico ya se encuentra registrado en el sistema.",StatusCodes.Status409Conflict);

            string passwordHash = _passwordHasher.Hash(request.Contrasena);

            var nuevoUsuario = new Domain.Entities.Usuario(
                Guid.NewGuid(),
                request.Nombre,
                request.Correo,
                RolesConstantes.Administrador,
                passwordHash);

            await _usuarioRepository.AgregarAsync(nuevoUsuario);

            return Result<string>.Success(_tokenService.GenerarToken(nuevoUsuario));
        }
    }
}