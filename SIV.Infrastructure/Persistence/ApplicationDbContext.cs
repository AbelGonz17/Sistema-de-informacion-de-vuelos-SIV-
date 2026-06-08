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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vuelo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NumeroVuelo).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Aerolinea).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Correo).IsRequired().HasMaxLength(150);
            });

            modelBuilder.Entity<LogAuditoria>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Usuario).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Accion).IsRequired().HasMaxLength(100);
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