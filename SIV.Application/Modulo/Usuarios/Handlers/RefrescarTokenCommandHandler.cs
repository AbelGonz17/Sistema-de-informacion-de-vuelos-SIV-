using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SIV.Application.Modulo.Usuarios.Commands;
using SIV.Application.Modulo.Usuarios.DTOs;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class RefrescarTokenCommandHandler : IRequestHandler<RefrescarTokenCommand, Result<TokenResponseDto>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public RefrescarTokenCommandHandler(IUsuarioRepository usuarioRepository, ITokenService tokenService, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<TokenResponseDto>> Handle(RefrescarTokenCommand request, CancellationToken cancellationToken)
        {
            var principal = GetPrincipalFromExpiredToken(request.AccessTokenViejo);
            if (principal == null)
            {
                return Result<TokenResponseDto>.Failure("Token de acceso inválido.", 400);
            }

            var userIdString = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Result<TokenResponseDto>.Failure("Token no contiene el ID del usuario.", 400);
            }

            var usuario = await _usuarioRepository.ObtenerParaModificacionAsync(userId);
            if (usuario == null || !usuario.Activo)
            {
                return Result<TokenResponseDto>.Failure("Usuario no encontrado o inactivo.", 404);
            }

            var refreshToken = usuario.RefreshTokens.FirstOrDefault(rt => rt.Token == request.RefreshTokenViejo);

            // Blindaje contra ataques de reutilización (Replay Attacks)
            if (refreshToken != null && refreshToken.Codificado)
            {
                // ¡Alarma! Se intentó usar un token revocado. Revocar TODOS.
                usuario.RevocarTodosRefreshTokens();
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result<TokenResponseDto>.Failure("Intento de reutilización de token detectado. Todas las sesiones han sido revocadas por seguridad.", 401);
            }

            if (refreshToken == null || !refreshToken.Activo)
            {
                return Result<TokenResponseDto>.Failure("Refresh Token inválido o expirado.", 401);
            }

            // Revocar el token viejo
            usuario.RevocarRefreshToken(request.RefreshTokenViejo);

            // Generar un nuevo par
            string nuevoAccessToken = _tokenService.GenerarToken(usuario);
            string nuevoRefreshToken = Guid.NewGuid().ToString();

            usuario.AgregarRefreshToken(nuevoRefreshToken, 7, request.IpAddress);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<TokenResponseDto>.Success(new TokenResponseDto(nuevoAccessToken, nuevoRefreshToken));
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "default_secret_key_needs_to_be_long_enough_for_hmacsha256")),
                ValidateIssuer = false, // Replace with true if you validate issuer
                ValidateAudience = false, // Replace with true if you validate audience
                ValidateLifetime = false // <-- CRUCIAL para poder refrescar
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null; // El token no está firmado con nuestro algoritmo esperado
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
