using SIV.Domain.Entities.Usuarios;

namespace SIV.Domain.Interfaces
{
    public interface ITokenService
    {
        string GenerarToken(Usuario usuario);
    }
}