using MediatR;
using SIV.Application.Modulo.Auditoria.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Modulo.Auditoria.Handlers
{
    public class ExportarAuditoriaCsvQueryHandler : IRequestHandler<ExportarAuditoriaCsvQuery, Result<byte[]>>
    {
        private readonly IAuditoriaRepository _auditoriaRepository;

        public ExportarAuditoriaCsvQueryHandler(IAuditoriaRepository auditoriaRepository)
        {
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task<Result<byte[]>> Handle(ExportarAuditoriaCsvQuery request, CancellationToken cancellationToken)
        {
            // For export we might want all records that match, so a large page size
            var (logs, _) = await _auditoriaRepository.ObtenerLogsPaginadosAsync(
                pageNumber: 1,
                pageSize: 100000, 
                request.FechaInicio,
                request.FechaFin,
                request.Accion
            );

            var sb = new StringBuilder();
            sb.AppendLine("Id,FechaHora,Usuario,Accion,Detalles");

            foreach (var l in logs)
            {
                // Escape commas and quotes for CSV
                var detalles = l.Detalles?.Replace("\"", "\"\"") ?? "";
                sb.AppendLine($"{l.Id},{l.FechaRegistro:yyyy-MM-dd HH:mm:ss},{l.Usuario ?? "Sistema"},{l.Accion},\"{detalles}\"");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            
            return Result<byte[]>.Success(bytes);
        }
    }
}
