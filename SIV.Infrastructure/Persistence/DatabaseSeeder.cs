using Microsoft.EntityFrameworkCore;
using SIV.Domain.Entities.Catalogo;

namespace SIV.Infrastructure.Persistence
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (!await context.Aerolineas.AnyAsync())
            {
                var aerolineas = new[]
                {
                    new Aerolinea { Id = Guid.NewGuid(), Nombre = "Arajet", Codigo = "DM" },
                    new Aerolinea { Id = Guid.NewGuid(), Nombre = "American Airlines", Codigo = "AA" },
                    new Aerolinea { Id = Guid.NewGuid(), Nombre = "Delta Airlines", Codigo = "DL" }
                };

                await context.Aerolineas.AddRangeAsync(aerolineas);
            }

            if (!await context.Aeropuertos.AnyAsync())
            {
                var aeropuertos = new[]
                {
                    new Aeropuerto { Id = Guid.NewGuid(), Nombre = "Las Américas International Airport", Codigo = "SDQ", Pais = "DO" },
                    new Aeropuerto { Id = Guid.NewGuid(), Nombre = "Punta Cana International Airport", Codigo = "PUJ", Pais = "DO" },
                    new Aeropuerto { Id = Guid.NewGuid(), Nombre = "Miami International Airport", Codigo = "MIA", Pais = "US" },
                    new Aeropuerto { Id = Guid.NewGuid(), Nombre = "John F. Kennedy International Airport", Codigo = "JFK", Pais = "US" }
                };

                await context.Aeropuertos.AddRangeAsync(aeropuertos);
            }

            if (context.ChangeTracker.HasChanges())
                await context.SaveChangesAsync();            
        }
    }
}