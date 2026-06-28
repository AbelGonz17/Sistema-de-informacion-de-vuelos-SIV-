using MediatR;
using SIV.Application.Common.Events;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Common.Behaviors
{
    public class AuditoriaBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ISeguridadService _seguridadService;
        private readonly IMediator _mediator;

        public AuditoriaBehavior(ISeguridadService seguridadService, IMediator mediator)
        {
            _seguridadService = seguridadService;
            _mediator = mediator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request.GetType().Name.EndsWith("Query"))
            {
                return await next();
            }

            var usuario = _seguridadService.ObtenerUsarioActual() ?? "Sistema_Local";
            var accion = request.GetType().Name;

            try
            {
                var response = await next();

                string resultadoDetalles;
                if (response is Result result && result.IsFailure)
                {
                    var customMsg = (request is IAuditableCommand auditable)
                        ? auditable.ObtenerMensajeAuditoria(response)
                        : $"Fallo al procesar {request.GetType().Name}.";
                    resultadoDetalles = $"{customMsg} Detalle del error: {result.ErrorMessage}";
                }
                else if (request is IAuditableCommand auditableCommand)
                {
                    resultadoDetalles = auditableCommand.ObtenerMensajeAuditoria(response);
                }
                else
                {
                    resultadoDetalles = "Éxito: Operación completada.";
                }

                var queryDetails = SerializeRequestSafely(request);
                var logMsg = $"Resultado: {resultadoDetalles}. Payload: {queryDetails}";

                await _mediator.Publish(new AuditoriaEvent(usuario, accion, logMsg), cancellationToken);

                return response;
            }
            catch (Exception ex)
            {
                var queryDetails = SerializeRequestSafely(request);
                var customMsg = (request is IAuditableCommand auditable)
                    ? auditable.ObtenerMensajeAuditoria(Result.Failure(ex.Message, 500))
                    : $"Fallo al ejecutar {request.GetType().Name}.";
                var logMsg = $"Fallo: Excepción lanzada: {ex.Message}. {customMsg} Payload: {queryDetails}";

                await _mediator.Publish(new AuditoriaEvent(usuario, accion, logMsg), cancellationToken);

                throw;
            }
        }

        private string SerializeRequestSafely(TRequest request)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(request);
                var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = false };

                var dictionary = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                if (dictionary != null)
                {
                    var keysToMask = new[] { "contrasena", "password", "clave", "token", "hash" };
                    foreach (var key in dictionary.Keys.ToList())
                    {
                        if (keysToMask.Any(k => key.Contains(k, StringComparison.OrdinalIgnoreCase)))
                        {
                            dictionary[key] = "***";
                        }
                    }
                    return System.Text.Json.JsonSerializer.Serialize(dictionary, options);
                }
                return json;
            }
            catch
            {
                return "[Error al serializar los detalles del request]";
            }
        }
    }
}
