using SIV.Domain.Common;

namespace SIV.Domain.Entities
{
    public class Vuelo
    {
        public Guid Id { get; private set; }
        public string NumeroVuelo { get; private set; }
        public string Aerolinea { get; private set; }
        public string Origen { get; private set; }
        public string Destino { get; private set; }
        public DateTime HorarioPlanificadoSalida { get; private set; }
        public DateTime HorarioPlanificadoLlegada { get; private set; }
        public DateTime? HorarioEstimadoSalida { get; private set; }
        public DateTime? HorarioEstimadoLlegada { get; private set; }
        public string Puerta { get; private set; }
        public string MotivoUltimoCambio { get; private set; }
        public EstadoVuelo EstadoActual { get; private set; }
        private Vuelo() { }

        public Vuelo(Guid id, 
            string numeroVuelo, 
            string aerolinea, 
            string origen, 
            string destino,
            DateTime horarioPlanificadoSalida, 
            DateTime horarioPlanificadoLlegada)
        {
            Id = id;
            NumeroVuelo = numeroVuelo;
            Aerolinea = aerolinea;
            Origen = origen;
            Destino = destino;
            HorarioPlanificadoSalida = horarioPlanificadoSalida;
            HorarioPlanificadoLlegada = horarioPlanificadoLlegada;
            EstadoActual = EstadoVuelo.Programado;
        }

        public void CambiarEstado(EstadoVuelo nuevoEstado)
        {
            if (EstadoActual == EstadoVuelo.Cancelado)
                throw new InvalidOperationException("Un vuelo en estado Cancelado es terminal e irreversible. No admite nuevos cambios.");
            
            if (EstadoActual == EstadoVuelo.Completado && nuevoEstado != EstadoVuelo.Completado)
                throw new InvalidOperationException("No se permiten retrocesos de estado ni modificaciones sobre un vuelo Completado.");
            
            if (nuevoEstado == EstadoVuelo.EnVuelo && EstadoActual == EstadoVuelo.Programado)
                throw new InvalidOperationException("Un vuelo no puede pasar a 'En Vuelo' sin haber pasado por el proceso de embarque.");
            
            EstadoActual = nuevoEstado;
        }

        public void RegistrarRetraso(DateTime nuevaHoraSalida, string motivo)
        {
            if (EstadoActual == EstadoVuelo.Cancelado || EstadoActual == EstadoVuelo.Completado)
                throw new InvalidOperationException("No se pueden registrar cambios operativos en vuelos cerrados o cancelados.");
            

            if (string.IsNullOrWhiteSpace(motivo))
                throw new InvalidOperationException("El motivo del retraso es obligatorio.");
            
            HorarioEstimadoSalida = nuevaHoraSalida;
            MotivoUltimoCambio = motivo;
            EstadoActual = EstadoVuelo.Retrasado; 
        }

        public void ActualizarPuerta(string nuevaPuerta)
        {
            Puerta = nuevaPuerta;
        }
    }
}
