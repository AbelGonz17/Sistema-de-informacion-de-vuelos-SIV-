using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Domain.Common;
using SIV.Domain.Entities.Vuelos;
using SIV.Domain.Interfaces;

namespace SIV.Application.Modulo.Vuelos.Handlers
{
    public class CrearVueloCommandHandler : IRequestHandler<CrearVueloCommand, Result<Guid>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly ISeguridadService _seguridadService;
        private readonly IMediator _mediator;
        private readonly IAerolineaRepository _aerolineaRepository;
        private readonly IAeropuertoRepository _aeropuertoRepository;

        public CrearVueloCommandHandler(
            IVueloRepository vueloRepository,
            ISeguridadService seguridadService,
            IMediator mediator,
            IAerolineaRepository aerolineaRepository,
            IAeropuertoRepository aeropuertoRepository)
        {
            _vueloRepository = vueloRepository;
            _seguridadService = seguridadService;
            _mediator = mediator;
            _aerolineaRepository = aerolineaRepository;
            _aeropuertoRepository = aeropuertoRepository;
        }

        public async Task<Result<Guid>> Handle(CrearVueloCommand request, CancellationToken cancellationToken)
        {
            var aerolinea = await _aerolineaRepository.ObtenerPorIdAsync(request.Aerolinea);
            if (aerolinea == null)
            {
                return Result<Guid>.Failure("La aerolínea especificada no existe.", StatusCodes.Status404NotFound);
            }

            var origen = await _aeropuertoRepository.ObtenerPorIdAsync(request.Origen);
            if (origen == null)
            {
                return Result<Guid>.Failure("El aeropuerto de origen especificado no existe.", StatusCodes.Status404NotFound);
            }

            var destino = await _aeropuertoRepository.ObtenerPorIdAsync(request.Destino);
            if (destino == null)
            {
                return Result<Guid>.Failure("El aeropuerto de destino especificado no existe.", StatusCodes.Status404NotFound);
            }

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

            var usuarioId = _seguridadService.ObtenerIdUsuarioActual();
            var usuarioNombre = _seguridadService.ObtenerUsarioActual();

            var nuevoVuelo = new Vuelo(
                Guid.NewGuid(),
                request.NumeroVuelo,
                request.Aerolinea,
                request.Origen,
                request.Destino,
                request.HorarioPlanificadoSalida,
                request.HorarioPlanificadoLlegada,
                request.Puerta,
                "Registro inicial del vuelo",
                usuarioId
            );

            await _vueloRepository.AgregarAsync(nuevoVuelo);

            await _mediator.Publish(new VueloCreadoEvent
            {
                VueloId = nuevoVuelo.Id,
                NumeroVuelo = nuevoVuelo.NumeroVuelo,
                Aerolinea = nuevoVuelo.Aerolinea,
                Origen = nuevoVuelo.Origen,
                Destino = nuevoVuelo.Destino,
                Usuario = usuarioNombre
            }, cancellationToken);

            return Result<Guid>.Success(nuevoVuelo.Id);
        }
    }
}