using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Domain.Common;

namespace SIV.Application.Common.Behaviors
{
    public class ValidacionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
          where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidacionBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                {
                    string mensajeError = failures.First().ErrorMessage; 

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
                                var resultadoFallo = metodoFalloGenerico.Invoke(null, new object[] { mensajeError, StatusCodes.Status400BadRequest });
                                return (TResponse)resultadoFallo!; 
                            }
                        }

                        return (TResponse)(object)Result.Failure(mensajeError, StatusCodes.Status400BadRequest);
                    }
                    throw new ValidationException(failures);
                }
            }
            return await next();
        }
    }
}