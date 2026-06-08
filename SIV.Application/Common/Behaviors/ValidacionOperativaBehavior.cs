using MediatR;
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
                    throw new UnauthorizedAccessException("El usuario no tiene permisos para registrar cambios operativos.");
                
                var vuelo = await _vueloRepository.ObtenerPorIdAsync(comandoOperativo.VueloId);
                if (vuelo == null)
                    throw new ArgumentException("El vuelo especificado no existe en el sistema.");
            }

            return await next();
        }
    }
}