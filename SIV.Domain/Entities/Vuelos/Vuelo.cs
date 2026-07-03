using SIV.Domain.Common;
using SIV.Domain.Entities.Catalogo;

namespace SIV.Domain.Entities.Vuelos
{
    public class Vuelo : ISoftDeletable
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
        public bool Activo { get; private set; } = true;

        private readonly List<HistorialEstado> _historialEstados = new();
        private readonly List<HistorialCambioOperativo> _historialCambio = new();

        public IReadOnlyCollection<HistorialEstado> HistorialEstados => _historialEstados.AsReadOnly();
        public IReadOnlyCollection<HistorialCambioOperativo> HistorialCambio => _historialCambio.AsReadOnly();
        
        private Vuelo() { }

        public Vuelo(Guid id,
            string numeroVuelo,
            Guid aerolinea,
            Guid origen,
            Guid destino,
            DateTime horarioPlanificadoSalida,
            DateTime horarioPlanificadoLlegada,
            string puerta,
            string motivoUltimoCambio,
            Guid usuarioResponsable)
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

            _historialEstados.Add(new HistorialEstado
            {
                Id = Guid.NewGuid(),
                VueloId = this.Id,
                EstadoAnterior = EstadoVuelo.Programado,
                EstadoNuevo = EstadoVuelo.Programado,
                FechaHora = DateTime.UtcNow,
                UsuarioResponsable = usuarioResponsable
            });
        }

        public void Desactivar()
        {
            Activo = false;
        }

        public void ActualizarDatosBasicos(
            Guid aerolinea, 
            Guid origen, 
            Guid destino, 
            DateTime horarioPlanificadoSalida, 
            DateTime horarioPlanificadoLlegada, 
            string puerta, 
            Guid usuarioResponsable)
        {
            if (EstadoActual != EstadoVuelo.Programado)
                throw new InvalidOperationException("Solo se pueden modificar los datos básicos de un vuelo en estado Programado.");

            if (HorarioEstimadoSalida.HasValue || HorarioEstimadoLlegada.HasValue)
                throw new InvalidOperationException("No se pueden modificar los datos básicos si ya existen estimaciones operativas registradas.");

            Aerolinea = aerolinea;
            Origen = origen;
            Destino = destino;
            HorarioPlanificadoSalida = horarioPlanificadoSalida;
            HorarioPlanificadoLlegada = horarioPlanificadoLlegada;
            Puerta = puerta;

            var historialCambio = new HistorialCambioOperativo
            {
                VueloId = this.Id,
                TipoCambio = "Corrección de Programación",
                Motivo = "Actualización de datos básicos antes de operación",
                DetalleCambio = "Se actualizaron los datos planificados del vuelo.",
                FechaHora = DateTime.UtcNow,
                UsuarioResponsable = usuarioResponsable
            };

            _historialCambio.Add(historialCambio);
        }

        public void CambiarEstado(EstadoVuelo nuevoEstado, string motivo, Guid usuarioResponsable)
        {
            if (EstadoActual == nuevoEstado)
                throw new InvalidOperationException($"El vuelo ya se encuentra en estado '{nuevoEstado}'.");

            if (EstadoActual == EstadoVuelo.Cancelado)
                throw new InvalidOperationException("Un vuelo en estado Cancelado es terminal e irreversible. No admite nuevos cambios.");

            if (EstadoActual == EstadoVuelo.Completado && nuevoEstado != EstadoVuelo.Completado)
                throw new InvalidOperationException("No se permiten retrocesos de estado ni modificaciones sobre un vuelo Completado.");

            if (nuevoEstado == EstadoVuelo.EnVuelo && EstadoActual == EstadoVuelo.Programado)
                throw new InvalidOperationException("Un vuelo no puede pasar a 'En Vuelo' sin haber pasado por el proceso de embarque.");

            var historial = new HistorialEstado
            {
                VueloId = this.Id,
                EstadoAnterior = this.EstadoActual,
                EstadoNuevo = nuevoEstado,
                FechaHora = DateTime.UtcNow,
                UsuarioResponsable = usuarioResponsable
            };

            _historialEstados.Add(historial);

            EstadoActual = nuevoEstado;

            if (!string.IsNullOrWhiteSpace(motivo))
            {
                MotivoUltimoCambio = motivo;
            }
        }

        public void ActualizarHorarioEstimado(DateTime nuevaHoraSalida, string motivo, Guid usuarioResponsable)
        {
            if (EstadoActual == EstadoVuelo.Cancelado || EstadoActual == EstadoVuelo.Completado)
                throw new InvalidOperationException("No se pueden registrar cambios operativos en vuelos cerrados o cancelados.");

            if (string.IsNullOrWhiteSpace(motivo))
                throw new InvalidOperationException("El motivo del cambio de horario es obligatorio.");

            TimeSpan duracionVuelo = HorarioPlanificadoLlegada - HorarioPlanificadoSalida;

            HorarioEstimadoSalida = nuevaHoraSalida;
            HorarioEstimadoLlegada = nuevaHoraSalida.Add(duracionVuelo);
        
            string tipoCambio;
            if (nuevaHoraSalida > HorarioPlanificadoSalida)
            {
                tipoCambio = "Retraso";
                EstadoActual = EstadoVuelo.Retrasado;
            }
            else if (nuevaHoraSalida < HorarioPlanificadoSalida)
            {
                tipoCambio = "Adelanto";
                EstadoActual = EstadoVuelo.Adelantado; 
            }
            else
            {
                tipoCambio = "Reprogramación";
                EstadoActual = EstadoVuelo.Programado; 
            }

            var historialCambio = new HistorialCambioOperativo
            {
                VueloId = this.Id,
                TipoCambio = tipoCambio,
                Motivo = motivo,
                DetalleCambio = $"Nuevo horario estimado de salida: {nuevaHoraSalida:O}",
                FechaHora = DateTime.UtcNow,
                UsuarioResponsable = usuarioResponsable
            };

            _historialCambio.Add(historialCambio);

            MotivoUltimoCambio = motivo;
        }

        public void ActualizarPuerta(string nuevaPuerta, string motivo, Guid usuarioResponsable)
        {
            if (EstadoActual == EstadoVuelo.Cancelado || EstadoActual == EstadoVuelo.Completado)
                throw new InvalidOperationException("No se puede cambiar la puerta de un vuelo cerrado o cancelado.");

            var historialCambio = new HistorialCambioOperativo
            {
                VueloId = this.Id,
                TipoCambio = "Cambio de Puerta",
                Motivo = string.IsNullOrWhiteSpace(motivo) ? "Cambio de puerta operativo" : motivo,
                DetalleCambio = $"La puerta cambió de '{Puerta}' a '{nuevaPuerta}'",
                FechaHora = DateTime.UtcNow,
                UsuarioResponsable = usuarioResponsable
            };

            _historialCambio.Add(historialCambio);

            Puerta = nuevaPuerta;

            if (!string.IsNullOrWhiteSpace(motivo))
            {
                MotivoUltimoCambio = motivo;
            }
        }
    }
}
