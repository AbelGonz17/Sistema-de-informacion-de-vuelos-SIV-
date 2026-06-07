namespace SIV.Domain.Interfaces
{
    public interface ISeguridadService
    {
        string ObtenerUsarioActual();
        string ObtenerRolUsuarioActual();
        bool ValidarRol(string rolRequerido);
    }
}