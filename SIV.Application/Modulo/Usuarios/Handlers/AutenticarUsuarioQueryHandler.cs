using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Modulo.Usuarios.Queries;
using SIV.Application.Modulo.Usuarios.DTOs;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;
using System.Security.Cryptography;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class AutenticarUsuarioQueryHandler : IRequestHandler<AutenticarUsuarioQuery, Result<TokenResponseDto>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;

        public AutenticarUsuarioQueryHandler(IUsuarioRepository usuarioRepository, ITokenService tokenService, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<TokenResponseDto>> Handle(AutenticarUsuarioQuery request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.ObtenerPorCorreoConRefreshTokensAsync(request.Correo);

            if (usuario == null)
                return Result<TokenResponseDto>.Failure("Las credenciales ingresadas son incorrectas.", StatusCodes.Status400BadRequest);

            if (!usuario.Activo)
                return Result<TokenResponseDto>.Failure("La cuenta de usuario ha sido desactivada.", StatusCodes.Status403Forbidden);

            if (usuario.BloqueadoHasta.HasValue && usuario.BloqueadoHasta.Value > DateTime.UtcNow)
            {
                var minutosRestantes = (usuario.BloqueadoHasta.Value - DateTime.UtcNow).TotalMinutes;
                return Result<TokenResponseDto>.Failure($"Cuenta bloqueada por demasiados intentos fallidos. Intente de nuevo en {Math.Ceiling(minutosRestantes)} minutos.", StatusCodes.Status403Forbidden);
            }

            bool contraseñaValida = _passwordHasher.Verify(request.Contrasena, usuario.PassWordHash);

            if (!contraseñaValida)
            {
                usuario.RegistrarIntentoFallido(limiteIntentos: 3, minutosBloqueo: 15);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result<TokenResponseDto>.Failure("Las credenciales ingresadas son incorrectas.", StatusCodes.Status400BadRequest);
            }

            usuario.ResetearIntentos();

            string accessToken = _tokenService.GenerarToken(usuario);
            string refreshToken = Guid.NewGuid().ToString();

            usuario.AgregarRefreshToken(refreshToken, 7, request.IpAddress);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new TokenResponseDto(accessToken, refreshToken);
            return Result<TokenResponseDto>.Success(response);
        }

    }
}