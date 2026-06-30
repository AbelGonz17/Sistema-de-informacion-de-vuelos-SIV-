using System;

namespace SIV.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime FechaExpiracion { get; set; }
        public bool Codificado { get; set; } // Representa si ha sido revocado/usado
        public bool Activo => DateTime.UtcNow <= FechaExpiracion && !Codificado;
        public DateTime FechaCreacion { get; set; }
        public string CreadoPorIp { get; set; } = string.Empty;
    }
}
