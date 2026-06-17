namespace SIV.Domain.Interfaces
{
    public interface ISeguridadService
    {
        string ObtenerUsarioActual();
        Guid ObtenerIdUsuarioActual();
        string ObtenerRolUsuarioActual();
        bool ValidarRol(string rolRequerido);
    }
}