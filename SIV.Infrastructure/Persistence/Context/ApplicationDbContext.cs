using Microsoft.EntityFrameworkCore;
using SIV.Domain.Entities;

namespace SIV.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Vuelo> Vuelos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<LogAuditoria> LogAuditorias { get; set; }
        public DbSet<Aerolinea> Aerolineas { get; set; }
        public DbSet<Aeropuerto> Aeropuertos { get; set; }
        public DbSet<HistorialEstado> HistorialEstados { get; set; }
        public DbSet<HistorialCambioOperativo> HistorialCambiosOperativos { get; set; }
        public DbSet<Seguimiento> Seguimientos { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Aerolinea>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(10);
                entity.HasIndex(e => e.Codigo).IsUnique();
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Aeropuerto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Pais).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Vuelo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NumeroVuelo).IsRequired().HasMaxLength(20);

                entity.HasOne(e => e.AerolineaRef)
                      .WithMany()
                      .HasForeignKey(e => e.Aerolinea)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.OrigenRef)
                      .WithMany()
                      .HasForeignKey(e => e.Origen)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.DestinoRef)
                      .WithMany()
                      .HasForeignKey(e => e.Destino)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Puerta).HasMaxLength(10);
                entity.Property(e => e.HorarioPlanificadoSalida).IsRequired();
                entity.Property(e => e.HorarioEstimadoSalida);

                entity.Property(e => e.EstadoActual)
                    .HasConversion<string>()
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Correo).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Rol).IsRequired().HasMaxLength(50);

                entity.Property(e => e.PassWordHash).IsRequired().HasMaxLength(255);
            });

            modelBuilder.Entity<HistorialCambioOperativo>(entity =>
            {
                entity.Property(e => e.TipoCambio)
                      .HasConversion<string>()
                      .IsRequired()
                      .HasMaxLength(50);
            });

            modelBuilder.Entity<LogAuditoria>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Usuario).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Accion).IsRequired().HasMaxLength(100);

                entity.Property(e => e.Detalles).HasMaxLength(500);
                entity.Property(e => e.FechaRegistro).IsRequired();
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<LogAuditoria>())
            {
                if (entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                {
                    throw new InvalidOperationException("Los registros de auditoría institucional son inmutables y de solo lectura.");
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}