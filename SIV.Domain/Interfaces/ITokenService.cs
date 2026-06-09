using SIV.Domain.Entities;

namespace SIV.Domain.Interfaces
{
    public interface ITokenService
    {
        string GenerarToken(Usuario usuario);
    }
}