using MediatR;
using Microsoft.Extensions.Logging;
using SIV.Application.Common.Interfaces;
using SIV.Domain.Interfaces;
using System;

namespace SIV.Application.Common.Behaviors
{
    public class TransaccionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TransaccionBehavior<TRequest, TResponse>> _logger;

        public TransaccionBehavior(IUnitOfWork unitOfWork, ILogger<TransaccionBehavior<TRequest, TResponse>> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is not IComandoOperativo)
            {
                return await next();
            }

            try
            {
                _logger.LogInformation("Iniciando transacción para el comando operativo {CommandName}", typeof(TRequest).Name);

                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                var response = await next();

                await _unitOfWork.CommitAsync(cancellationToken);
                _logger.LogInformation("Transacción confirmada exitosamente para {CommandName}", typeof(TRequest).Name);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción detectada en el flujo operativo. Aplicando Rollback automático para {CommandName}", typeof(TRequest).Name);

                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
