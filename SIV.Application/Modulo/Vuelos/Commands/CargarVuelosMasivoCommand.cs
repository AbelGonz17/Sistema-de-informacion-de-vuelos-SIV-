using MediatR;
using Microsoft.AspNetCore.Http;
using SIV.Application.Common.Interfaces;
using SIV.Application.Modulo.Vuelos.DTOs;
using SIV.Domain.Common;

namespace SIV.Application.Modulo.Vuelos.Commands
{
    public class CargarVuelosMasivoCommand : IRequest<Result<ResultadoCargaMasivaDto>>, IComandoCatalogo, IAuditableCommand
    {
        public IFormFile Archivo { get; set; } = null!;

        public string ObtenerMensajeAuditoria(object response)
        {
            if (response is Result<ResultadoCargaMasivaDto> result && result.IsSuccess)
            {
                return $"Se procesó una carga masiva de vuelos. Éxitos: {result.Value.TotalExitosos}, Errores: {result.Value.TotalErrores}.";
            }
            return "Intento de carga masiva de vuelos fallido.";
        }
    }
}
