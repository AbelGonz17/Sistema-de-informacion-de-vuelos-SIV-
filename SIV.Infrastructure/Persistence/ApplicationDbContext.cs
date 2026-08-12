using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using SIV.Domain.Entities.Catalogo;
using SIV.Domain.Entities.Sistema;
using SIV.Domain.Entities.Usuarios;
using SIV.Domain.Entities.Vuelos;

namespace SIV.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor = null) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
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
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
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

                entity.OwnsMany(e => e.RefreshTokens, rt =>
                {
                    rt.ToTable("RefreshTokens");
                    rt.HasKey(t => t.Id);
                    rt.Property(t => t.Token).IsRequired().HasMaxLength(200);
                    rt.Property(t => t.CreadoPorIp).HasMaxLength(50);
                });
            });

            modelBuilder.Entity<LogAuditoria>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Usuario).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Accion).IsRequired().HasMaxLength(100);

                entity.Property(e => e.Detalles).HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.FechaRegistro).IsRequired();
            });

            modelBuilder.Entity<Notificacion>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Mensaje).IsRequired().HasMaxLength(500);
                entity.Property(e => e.TipoEvento)
                      .HasConversion<string>()
                      .IsRequired()
                      .HasMaxLength(50);
            });

            modelBuilder.Entity<Usuario>().HasQueryFilter(x => x.Activo);
            modelBuilder.Entity<Aerolinea>().HasQueryFilter(x => x.Activo);
            modelBuilder.Entity<Aeropuerto>().HasQueryFilter(x => x.Activo);
            modelBuilder.Entity<Vuelo>().HasQueryFilter(x => x.Activo);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<LogAuditoria>())
            {
                if (entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                {
                    throw new InvalidOperationException("Los registros de auditoría institucional son inmutables y de solo lectura.");
                }
            }

            var auditEntries = new List<LogAuditoria>();
            var user = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "Sistema";

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is LogAuditoria || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var entityName = entry.Entity.GetType().Name;
                if (entityName != "Vuelo" && entityName != "Usuario" && entityName != "Aerolinea" && entityName != "Aeropuerto")
                    continue;

                var accion = entry.State switch
                {
                    EntityState.Added => $"Crear{entityName}",
                    EntityState.Modified => $"Editar{entityName}",
                    EntityState.Deleted => $"Eliminar{entityName}",
                    _ => entry.State.ToString()
                };

                var oldValues = new Dictionary<string, object>();
                var newValues = new Dictionary<string, object>();

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary) continue;
                    
                    string propertyName = property.Metadata.Name;

                    if (entry.State == EntityState.Added)
                    {
                        newValues[propertyName] = property.CurrentValue;
                    }
                    else if (entry.State == EntityState.Deleted)
                    {
                        oldValues[propertyName] = property.OriginalValue;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        if (property.IsModified)
                        {
                            oldValues[propertyName] = property.OriginalValue;
                            newValues[propertyName] = property.CurrentValue;
                        }
                    }
                }

                var keyName = entry.Metadata.FindPrimaryKey()?.Properties.Select(x => x.Name).SingleOrDefault();
                var entityId = keyName != null ? entry.Property(keyName).CurrentValue?.ToString() ?? "0" : "0";

                var detallesJson = JsonSerializer.Serialize(new { 
                    Entidad = entityName, 
                    EntidadId = entityId, 
                    ValoresAnteriores = oldValues, 
                    ValoresNuevos = newValues 
                });

                var auditEntry = new LogAuditoria(Guid.NewGuid(), user, accion, detallesJson);
                auditEntries.Add(auditEntry);
            }

            if (auditEntries.Any())
            {
                LogAuditorias.AddRange(auditEntries);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}