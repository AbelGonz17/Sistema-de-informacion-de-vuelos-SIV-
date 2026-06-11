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

        public void RegistrarRetraso(DateTime nuevaHoraSalida, string motivo)
        {
            if (EstadoActual == EstadoVuelo.Cancelado || EstadoActual == EstadoVuelo.Completado)
                throw new InvalidOperationException("No se pueden registrar cambios operativos en vuelos cerrados o cancelados.");

            if (string.IsNullOrWhiteSpace(motivo))
                throw new InvalidOperationException("El motivo del retraso es obligatorio.");

            TimeSpan duracionVuelo = HorarioPlanificadoLlegada - HorarioPlanificadoSalida;

            HorarioEstimadoSalida = nuevaHoraSalida;
            HorarioEstimadoLlegada = nuevaHoraSalida.Add(duracionVuelo);

            MotivoUltimoCambio = motivo;
            EstadoActual = EstadoVuelo.Retrasado;
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
