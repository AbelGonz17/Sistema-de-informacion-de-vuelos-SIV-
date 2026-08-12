using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Modulo.Vuelos.Commands;
using SIV.Application.Modulo.Vuelos.DTOs;
using SIV.Domain.Common;
using SIV.Domain.Entities.Vuelos;
using SIV.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIV.Application.Modulo.Vuelos.Handlers
{
    public class CargarVuelosMasivoCommandHandler : IRequestHandler<CargarVuelosMasivoCommand, Result<ResultadoCargaMasivaDto>>
    {
        private readonly IVueloRepository _vueloRepository;
        private readonly ISeguridadService _seguridadService;
        private readonly IAerolineaRepository _aerolineaRepository;
        private readonly IAeropuertoRepository _aeropuertoRepository;
        private readonly IMediator _mediator;

        public CargarVuelosMasivoCommandHandler(
            IVueloRepository vueloRepository,
            ISeguridadService seguridadService,
            IAerolineaRepository aerolineaRepository,
            IAeropuertoRepository aeropuertoRepository,
            IMediator mediator)
        {
            _vueloRepository = vueloRepository;
            _seguridadService = seguridadService;
            _aerolineaRepository = aerolineaRepository;
            _aeropuertoRepository = aeropuertoRepository;
            _mediator = mediator;
        }

        public async Task<Result<ResultadoCargaMasivaDto>> Handle(CargarVuelosMasivoCommand request, CancellationToken cancellationToken)
        {
            var resultado = new ResultadoCargaMasivaDto();

            if (request.Archivo == null || request.Archivo.Length == 0)
            {
                return Result<ResultadoCargaMasivaDto>.Failure("El archivo proporcionado está vacío o no es válido.");
            }

            var extension = Path.GetExtension(request.Archivo.FileName).ToLower();
            if (extension != ".xlsx")
            {
                return Result<ResultadoCargaMasivaDto>.Failure("El formato del archivo debe ser .xlsx");
            }

            using var stream = request.Archivo.OpenReadStream();
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet(1);
            var rows = worksheet.RangeUsed().RowsUsed();

            bool isFirstRow = true;
            var usuarioId = _seguridadService.ObtenerIdUsuarioActual();
            var usuarioNombre = _seguridadService.ObtenerUsarioActual();

            var aerolineas = await _aerolineaRepository.ObtenerTodasAsync();
            var aeropuertos = await _aeropuertoRepository.ObtenerTodosAsync();
            
            var aerolineaDict = new Dictionary<string, Guid>();
            foreach (var a in aerolineas)
            {
                if (!string.IsNullOrEmpty(a.Nombre)) aerolineaDict.TryAdd(a.Nombre.Trim().ToLowerInvariant(), a.Id);
                if (!string.IsNullOrEmpty(a.Codigo)) aerolineaDict.TryAdd(a.Codigo.Trim().ToLowerInvariant(), a.Id);
            }

            var aeropuertoDict = new Dictionary<string, Guid>();
            foreach (var a in aeropuertos)
            {
                if (!string.IsNullOrEmpty(a.Nombre)) aeropuertoDict.TryAdd(a.Nombre.Trim().ToLowerInvariant(), a.Id);
                if (!string.IsNullOrEmpty(a.Codigo)) aeropuertoDict.TryAdd(a.Codigo.Trim().ToLowerInvariant(), a.Id);
            }

            foreach (var row in rows)
            {
                if (isFirstRow)
                {
                    isFirstRow = false; // Saltar encabezados
                    continue;
                }

                resultado.TotalProcesados++;
                var numeroFila = row.RowNumber();

                try
                {
                    // Obtener valores
                    string numeroVuelo = row.Cell(1).GetValue<string>();
                    string aerolineaStr = row.Cell(2).GetValue<string>()?.Trim().ToLowerInvariant();
                    string origenStr = row.Cell(3).GetValue<string>()?.Trim().ToLowerInvariant();
                    string destinoStr = row.Cell(4).GetValue<string>()?.Trim().ToLowerInvariant();
                    
                    if (!DateTime.TryParse(row.Cell(5).GetValue<string>(), out DateTime salida) && !row.Cell(5).TryGetValue<DateTime>(out salida))
                    {
                        AgregarError(resultado, numeroFila, "Formato de fecha de salida inválido.");
                        continue;
                    }
                    
                    if (!DateTime.TryParse(row.Cell(6).GetValue<string>(), out DateTime llegada) && !row.Cell(6).TryGetValue<DateTime>(out llegada))
                    {
                        AgregarError(resultado, numeroFila, "Formato de fecha de llegada inválido.");
                        continue;
                    }

                    string puerta = row.Cell(7).GetValue<string>();

                    if (string.IsNullOrEmpty(aerolineaStr) || !aerolineaDict.TryGetValue(aerolineaStr, out Guid aerolineaId))
                    {
                        AgregarError(resultado, numeroFila, "No se encontró la aerolínea con el nombre o código proporcionado.");
                        continue;
                    }

                    if (string.IsNullOrEmpty(origenStr) || !aeropuertoDict.TryGetValue(origenStr, out Guid origenId))
                    {
                        AgregarError(resultado, numeroFila, "No se encontró el aeropuerto de origen con el nombre o código proporcionado.");
                        continue;
                    }

                    if (string.IsNullOrEmpty(destinoStr) || !aeropuertoDict.TryGetValue(destinoStr, out Guid destinoId))
                    {
                        AgregarError(resultado, numeroFila, "No se encontró el aeropuerto de destino con el nombre o código proporcionado.");
                        continue;
                    }

                    bool existeVueloDuplicado = await _vueloRepository.ExisteVueloAsync(
                        numeroVuelo,
                        aerolineaId,
                        salida.Date,
                        origenId,
                        destinoId
                    );

                    if (existeVueloDuplicado)
                    {
                        AgregarError(resultado, numeroFila, "Ya existe un vuelo programado con ese número, aerolínea y ruta para la fecha especificada.");
                        continue;
                    }

                    var nuevoVuelo = new Vuelo(
                        Guid.NewGuid(),
                        numeroVuelo,
                        aerolineaId,
                        origenId,
                        destinoId,
                        salida,
                        llegada,
                        puerta,
                        "Registro masivo de vuelo",
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

                    resultado.TotalExitosos++;
                }
                catch (Exception ex)
                {
                    AgregarError(resultado, numeroFila, $"Error inesperado al procesar la fila: {ex.Message}");
                }
            }

            return Result<ResultadoCargaMasivaDto>.Success(resultado);
        }

        private void AgregarError(ResultadoCargaMasivaDto resultado, int fila, string mensaje)
        {
            resultado.TotalErrores++;
            resultado.Errores.Add(new ErrorFilaDto
            {
                Fila = fila,
                Mensaje = mensaje
            });
        }
    }
}
