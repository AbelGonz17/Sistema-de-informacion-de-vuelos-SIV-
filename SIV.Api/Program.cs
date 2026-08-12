using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SIV.Application;
using SIV.Infrastructure;
using SIV.Infrastructure.Persistence;
using SIV.Infrastructure.RealTime;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------------------
// 1. Configuración de Servicios
// -----------------------------------------------------------------------------

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "KeySuperSeguraPorDefecto1234567890")),

        NameClaimType = "name",
        RoleClaimType = System.Security.Claims.ClaimTypes.Role,
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Fallo de autenticación: {context.Exception.Message}");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SIV API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa el token JWT de esta manera: Bearer {tu_token_aqui}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.SetIsOriginAllowed(origin => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// -----------------------------------------------------------------------------
// 2. Pipeline de HTTP Middleware
// -----------------------------------------------------------------------------

app.UseMiddleware<SIV.Api.Middleware.GlobalExceptionMiddleware>();

// Habilitar Swagger siempre (o mantener solo si es Development/Production)
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SIV API v1");
        c.RoutePrefix = "swagger"; // Acceso en /swagger
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<FidsHub>("/vuelosHub");

// -----------------------------------------------------------------------------
// 3. Migraciones automáticas y Seed Data con reintentos para Docker
// -----------------------------------------------------------------------------

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    int maxRetries = 5;
    int delaySeconds = 5;

    for (int retry = 1; retry <= maxRetries; retry++)
    {
        try
        {
            logger.LogInformation($"Intentando conectar a SQL Server y aplicar migraciones (Intento {retry}/{maxRetries})...");
            var context = services.GetRequiredService<ApplicationDbContext>();

            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                logger.LogInformation("Aplicando migraciones pendientes...");
                await context.Database.MigrateAsync();
            }

            await DatabaseSeeder.SeedAsync(context);
            logger.LogInformation("Base de datos sincronizada y datos iniciales (Seed) cargados exitosamente.");
            break; // Conexión exitosa, salir del bucle
        }
        catch (Exception ex)
        {
            logger.LogWarning($"Intento {retry} fallido: {ex.Message}");

            if (retry == maxRetries)
            {
                logger.LogError(ex, "Ocurrió un error crítico al aplicar las migraciones automáticas tras múltiples intentos.");
            }
            else
            {
                logger.LogInformation($"Esperando {delaySeconds} segundos antes de reintentar...");
                await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
            }
        }
    }
}

app.Run();