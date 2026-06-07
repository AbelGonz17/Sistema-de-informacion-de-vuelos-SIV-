using SIV.Domain.Entities;

namespace SIV.Domain.Interfaces
{
    public interface IAuditoriaRepository
    {
        Task RegistrarLogAsync(LogAuditoria log);
    }
}