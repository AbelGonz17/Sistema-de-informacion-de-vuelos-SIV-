using Microsoft.AspNetCore.Http;

namespace SIV.Application.Modulo.Vuelos.DTOs
{
    public class CargarVuelosMasivoRequest
    {
        public IFormFile File { get; set; } = null!;
    }
}