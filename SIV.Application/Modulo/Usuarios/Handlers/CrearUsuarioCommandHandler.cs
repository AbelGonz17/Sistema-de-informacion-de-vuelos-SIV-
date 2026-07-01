using MediatR;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Domain.Common;
using SIV.Domain.Entities.Usuarios;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class CrearUsuarioCommandHandler : IRequestHandler<CrearUsuarioInternoCommand, Result<string>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public CrearUsuarioCommandHandler(IUsuarioRepository usuarioRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<Result<string>> Handle(CrearUsuarioInternoCommand request, CancellationToken cancellationToken)
        {
            var existeCorreo = await _usuarioRepository.ObtenerPorCorreoAsync(request.CorreoElectronico);

            if (existeCorreo != null)   
                return Result<string>.Failure($"El correo electrónico '{request.CorreoElectronico}' ya se encuentra registrado.");

            string passwordHash = _passwordHasher.Hash(request.Contrasena);

            var nuevoUsuarioInterno = new Usuario
            (
                Guid.NewGuid(),
                request.Nombre,
                request.CorreoElectronico,
                request.Rol,
                passwordHash
            );

            await _usuarioRepository.AgregarAsync(nuevoUsuarioInterno);

            return Result<string>.Success(_tokenService.GenerarToken(nuevoUsuarioInterno));
        }
    }
}