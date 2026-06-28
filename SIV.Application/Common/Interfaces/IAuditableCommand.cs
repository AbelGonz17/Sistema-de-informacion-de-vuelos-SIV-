namespace SIV.Application.Common.Interfaces
{
    public interface IAuditableCommand
    {
        string ObtenerMensajeAuditoria(object response);
    }
}
