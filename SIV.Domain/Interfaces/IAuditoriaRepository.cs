using SIV.Domain.Entities.Sistema;

namespace SIV.Domain.Interfaces
{
    public interface IAuditoriaRepository
    {
        Task RegistrarLogAsync(LogAuditoria log);
        Task<(IEnumerable<LogAuditoria> Logs, int TotalCount)> ObtenerLogsPaginadosAsync(int pageNumber, int pageSize, DateTime? fechaInicio, DateTime? fechaFin, string? accion);
    }
}