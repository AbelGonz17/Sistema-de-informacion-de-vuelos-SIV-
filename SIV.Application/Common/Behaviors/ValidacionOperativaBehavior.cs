using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Common.Behaviors
{
    public class ValidacionOperativaBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ISeguridadService _seguridadService;
        private readonly IVueloRepository _vueloRepository;

        public ValidacionOperativaBehavior(ISeguridadService seguridadService, IVueloRepository vueloRepository)
        {
            _seguridadService = seguridadService;
            _vueloRepository = vueloRepository;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is IComandoOperativo comandoOperativo)
            {
                if (!_seguridadService.ValidarRol(RolesConstantes.Operador) &&
                    !_seguridadService.ValidarRol(RolesConstantes.Administrador))
                {
                    return CreateFailureResult("El usuario no tiene permisos para registrar cambios operativos.", StatusCodes.Status401Unauthorized);
                }

                var vuelo = await _vueloRepository.ObtenerPorIdAsync(comandoOperativo.VueloId);
                if (vuelo == null)
                {
                    return CreateFailureResult("El vuelo especificado no existe en el sistema.", StatusCodes.Status404NotFound);
                }
            }
            return await next();
        }

        private TResponse CreateFailureResult(string mensajeError, int statusCode)
        {
            if (typeof(Result).IsAssignableFrom(typeof(TResponse)))
            {
                if (typeof(TResponse).IsGenericType)
                {
                    var tipoResultadoContenido = typeof(TResponse).GetGenericArguments()[0];
                    var metodoFalloGenerico = typeof(Result<>)
                        .MakeGenericType(tipoResultadoContenido)
                        .GetMethod(nameof(Result.Failure), new[] { typeof(string), typeof(int) });

                    if (metodoFalloGenerico != null)
                    {
                        var resultadoFallo = metodoFalloGenerico.Invoke(null, new object[] { mensajeError, statusCode });
                        return (TResponse)resultadoFallo!;
                    }
                }
                return (TResponse)(object)Result.Failure(mensajeError, statusCode);
            }
            throw new UnauthorizedAccessException(mensajeError);
        }
    }
}