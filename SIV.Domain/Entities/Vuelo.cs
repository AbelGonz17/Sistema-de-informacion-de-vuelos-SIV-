using SIV.Domain.Common;

namespace SIV.Domain.Entities
{
    public class Vuelo
    {
        public Guid Id { get; private set; }
        public string NumeroVuelo { get; private set; }
        public Guid Aerolinea { get; private set; }
        public Guid Origen { get; private set; }
        public Guid Destino { get; private set; }
        public DateTime HorarioPlanificadoSalida { get; private set; }
        public DateTime HorarioPlanificadoLlegada { get; private set; }
        public DateTime? HorarioEstimadoSalida { get; private set; }
        public DateTime? HorarioEstimadoLlegada { get; private set; }
        public string Puerta { get; private set; }
        public string MotivoUltimoCambio { get; private set; }
        public EstadoVuelo EstadoActual { get; private set; }
        public Aerolinea AerolineaRef { get; set; }
        public Aeropuerto OrigenRef { get; set; }
        public Aeropuerto DestinoRef { get; set; }

        public IReadOnlyCollection<HistorialEstado> HistorialEstados { get; set; }
        public IReadOnlyCollection<HistorialCambioOperativo> HistorialCambio { get; set; }
        private Vuelo() { }

        public Vuelo(Guid id,
            string numeroVuelo,
            Guid aerolinea,
            Guid origen,
            Guid destino,
            DateTime horarioPlanificadoSalida,
            DateTime horarioPlanificadoLlegada,
            string puerta,
            string motivoUltimoCambio)
        {
            Id = id;
            NumeroVuelo = numeroVuelo;
            Aerolinea = aerolinea;
            Origen = origen;
            Destino = destino;
            HorarioPlanificadoSalida = horarioPlanificadoSalida;
            HorarioPlanificadoLlegada = horarioPlanificadoLlegada;
            Puerta = puerta;
            EstadoActual = EstadoVuelo.Programado;
            MotivoUltimoCambio = motivoUltimoCambio;
        }

        public void CambiarEstado(EstadoVuelo nuevoEstado, string motivo)
        {
            if (EstadoActual == EstadoVuelo.Cancelado)
                throw new InvalidOperationException("Un vuelo en estado Cancelado es terminal e irreversible. No admite nuevos cambios.");

            if (EstadoActual == EstadoVuelo.Completado && nuevoEstado != EstadoVuelo.Completado)
                throw new InvalidOperationException("No se permiten retrocesos de estado ni modificaciones sobre un vuelo Completado.");

            if (nuevoEstado == EstadoVuelo.EnVuelo && EstadoActual == EstadoVuelo.Programado)
                throw new InvalidOperationException("Un vuelo no puede pasar a 'En Vuelo' sin haber pasado por el proceso de embarque.");

            EstadoActual = nuevoEstado;

            if (!string.IsNullOrWhiteSpace(motivo))
            {
                MotivoUltimoCambio = motivo;
            }
        }

        public void ActualizarHorarioEstimado(DateTime nuevaHoraSalida, string motivo)
        {
            if (EstadoActual == EstadoVuelo.Cancelado || EstadoActual == EstadoVuelo.Completado)
                throw new InvalidOperationException("No se pueden registrar cambios operativos en vuelos cerrados o cancelados.");

            if (string.IsNullOrWhiteSpace(motivo))
                throw new InvalidOperationException("El motivo del cambio de horario es obligatorio.");

            TimeSpan duracionVuelo = HorarioPlanificadoLlegada - HorarioPlanificadoSalida;

            HorarioEstimadoSalida = nuevaHoraSalida;
            HorarioEstimadoLlegada = nuevaHoraSalida.Add(duracionVuelo);
        
            if (nuevaHoraSalida > HorarioPlanificadoSalida)
            {
                EstadoActual = EstadoVuelo.Retrasado;
            }
            else if (nuevaHoraSalida < HorarioPlanificadoSalida)
            {
                EstadoActual = EstadoVuelo.Adelantado; 
            }
            else
            {
                EstadoActual = EstadoVuelo.Programado; 
            }

            MotivoUltimoCambio = motivo;
        }

        public void ActualizarPuerta(string nuevaPuerta, string motivo)
        {
            if (EstadoActual == EstadoVuelo.Cancelado || EstadoActual == EstadoVuelo.Completado)
                throw new InvalidOperationException("No se puede cambiar la puerta de un vuelo cerrado o cancelado.");

            Puerta = nuevaPuerta;

            if (!string.IsNullOrWhiteSpace(motivo))
            {
                MotivoUltimoCambio = motivo;
            }
        }
    }
}
