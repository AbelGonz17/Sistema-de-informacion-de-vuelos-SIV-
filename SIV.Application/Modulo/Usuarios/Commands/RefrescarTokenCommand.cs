using MediatR;
using SIV.Application.Modulo.Usuarios.DTOs;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Usuarios.Commands
{
    public record RefrescarTokenCommand(string AccessTokenViejo, string RefreshTokenViejo, string IpAddress) 
        : IRequest<Result<TokenResponseDto>>;
}
