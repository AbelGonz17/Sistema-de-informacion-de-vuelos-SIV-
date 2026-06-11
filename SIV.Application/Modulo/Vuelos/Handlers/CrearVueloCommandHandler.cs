using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Domain.Common;
using SIV.Domain.Entities;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Vuelos.Handlers
{
    public class CrearVueloCommandHandler : IRequestHandler<CrearVueloCommand, Result<Guid>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly ISeguridadService _seguridadService;
        private readonly IMediator _mediator;

        public CrearVueloCommandHandler(
            IVueloRepository vueloRepository,
            ISeguridadService seguridadService,
            IMediator mediator)
        {
            _vueloRepository = vueloRepository;
            _seguridadService = seguridadService;
            _mediator = mediator;
        }

        public async Task<Result<Guid>> Handle(CrearVueloCommand request, CancellationToken cancellationToken)
        {
            bool existeVueloDuplicado = await _vueloRepository.ExisteVueloAsync(
                request.NumeroVuelo,
                request.Aerolinea,
                request.HorarioPlanificadoSalida.Date,
                request.Origen,
                request.Destino
            );

            if (existeVueloDuplicado)
            {
                return Result<Guid>.Failure(
                    "Ya existe un vuelo programado con ese número, aerolínea y ruta para la fecha especificada.",
                    StatusCodes.Status400BadRequest
                );
            }

            var nuevoVuelo = new Vuelo(
                Guid.NewGuid(),
                request.NumeroVuelo,
                request.Aerolinea,
                request.Origen,
                request.Destino,
                request.HorarioPlanificadoSalida,
                request.HorarioPlanificadoLlegada,
                request.Puerta,
                "Registro inicial del vuelo"
            );

            await _vueloRepository.AgregarAsync(nuevoVuelo);

            var usuarioActual = _seguridadService.ObtenerUsarioActual();

            await _mediator.Publish(new VueloCreadoEvent
            {
                VueloId = nuevoVuelo.Id,
                NumeroVuelo = nuevoVuelo.NumeroVuelo,
                Aerolinea = nuevoVuelo.Aerolinea,
                Origen = nuevoVuelo.Origen,
                Destino = nuevoVuelo.Destino,
                Usuario = usuarioActual
            }, cancellationToken);

            return Result<Guid>.Success(nuevoVuelo.Id);
        }
    }
}