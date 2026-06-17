using Microsoft.AspNetCore.Http;
using SIV.Domain.Interfaces;
using System.Security.Claims;

namespace SIV.Infrastructure.Security
{
    public class SeguridadService : ISeguridadService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SeguridadService(IHttpContextAccessor httpContectAccesor)
        {
            _httpContextAccessor = httpContectAccesor;
        }
        public string ObtenerUsarioActual()
        {
            var usuario = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
            return usuario ?? "Sistema_Local"; 
        }

        public Guid ObtenerIdUsuarioActual()
        {
            var idString = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(idString, out var id))
            {
                return id;
            }
            return Guid.Empty; // Guid.Empty si es un proceso automático (background)
        }

        public string ObtenerRolUsuarioActual()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }
        public bool ValidarRol(string rolRequerido)
        {
            var usuarioJson = _httpContextAccessor.HttpContext?.User;
            if (usuarioJson == null) return false;

            return usuarioJson.IsInRole(rolRequerido);
        }
    }
}