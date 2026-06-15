using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Common;
using SIV.Domain.Interfaces;

namespace SIV.Application.Common.Behaviors
{
    public class ValidacionSeguimientoBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ISeguridadService _seguridadService;
        private readonly IVueloRepository _vueloRepository;

        public ValidacionSeguimientoBehavior(ISeguridadService seguridadService, IVueloRepository vueloRepository)
        {
            _seguridadService = seguridadService;
            _vueloRepository = vueloRepository;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is IComandoSeguimiento comandoSeguimiento)
            {
                var usuarioActual = _seguridadService.ObtenerUsarioActual();
                if (string.IsNullOrEmpty(usuarioActual))          
                    return CreateFailureResult("Debe iniciar sesión para poder realizar el seguimiento de vuelos.", StatusCodes.Status401Unauthorized);
                

                var vuelo = await _vueloRepository.ObtenerPorIdAsync(comandoSeguimiento.VueloId);
                if (vuelo == null)              
                    return CreateFailureResult("El vuelo especificado al que desea dar seguimiento no existe.", StatusCodes.Status404NotFound);             
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