using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Modulo.Usuarios.DTOs;
using SIV.Application.Modulo.Usuarios.Queries;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Modulo.Usuarios.Handlers
{
    public class ConsultarNotificacionesUsuarioQueryHandler : IRequestHandler<ConsultarNotificacionesUsuarioQuery, Result<IEnumerable<NotificacionDto>>>
    {
        private readonly INotificacionRepository _notificacionRepository;

        public ConsultarNotificacionesUsuarioQueryHandler(INotificacionRepository notificacionRepository)
        {
            _notificacionRepository = notificacionRepository;
        }

        public async Task<Result<IEnumerable<NotificacionDto>>> Handle(ConsultarNotificacionesUsuarioQuery request, CancellationToken cancellationToken)
        {
            var notificaciones = await _notificacionRepository.ObtenerPorUsuarioAsync(request.UsuarioId);

            var result = notificaciones.Select(n => new NotificacionDto
            {
                Id = n.Id,
                VueloRelacionadoId = n.VueloRelacionadoId,
                TipoEvento = n.TipoEvento.ToString(),
                Mensaje = n.Mensaje,
                FechaHoraGenearicion = n.FechaHoraGenearicion,
                FueLeida = n.FueLeida
            }).ToList();

            return Result<IEnumerable<NotificacionDto>>.Success(result);
        }
    }
}
