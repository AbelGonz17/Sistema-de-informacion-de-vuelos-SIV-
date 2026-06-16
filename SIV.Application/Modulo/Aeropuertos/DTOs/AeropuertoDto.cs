using System;

namespace SIV.Application.Modulo.Aeropuertos.DTOs
{
    public class AeropuertoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
    }
}
