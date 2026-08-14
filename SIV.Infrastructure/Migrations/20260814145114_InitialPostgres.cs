using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIV.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aerolineas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aerolineas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Aeropuertos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Pais = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aeropuertos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogAuditorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Accion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Detalles = table.Column<string>(type: "text", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogAuditorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Correo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Rol = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PassWordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    IntentosFallidosLogin = table.Column<int>(type: "integer", nullable: false),
                    BloqueadoHasta = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResetPasswordToken = table.Column<string>(type: "text", nullable: true),
                    ResetPasswordTokenExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vuelos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NumeroVuelo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Aerolinea = table.Column<Guid>(type: "uuid", nullable: false),
                    Origen = table.Column<Guid>(type: "uuid", nullable: false),
                    Destino = table.Column<Guid>(type: "uuid", nullable: false),
                    HorarioPlanificadoSalida = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HorarioPlanificadoLlegada = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HorarioEstimadoSalida = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HorarioEstimadoLlegada = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Puerta = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    MotivoUltimoCambio = table.Column<string>(type: "text", nullable: false),
                    EstadoActual = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vuelos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vuelos_Aerolineas_Aerolinea",
                        column: x => x.Aerolinea,
                        principalTable: "Aerolineas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vuelos_Aeropuertos_Destino",
                        column: x => x.Destino,
                        principalTable: "Aeropuertos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vuelos_Aeropuertos_Origen",
                        column: x => x.Origen,
                        principalTable: "Aeropuertos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notificaciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioDestinatarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    VueloRelacionadoId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoEvento = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Mensaje = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FechaHoraGenearicion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FueLeida = table.Column<bool>(type: "boolean", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notificaciones_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    FechaExpiracion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Codificado = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreadoPorIp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistorialCambiosOperativos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VueloId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoCambio = table.Column<string>(type: "text", nullable: false),
                    Motivo = table.Column<string>(type: "text", nullable: false),
                    DetalleCambio = table.Column<string>(type: "text", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioResponsable = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialCambiosOperativos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialCambiosOperativos_Vuelos_VueloId",
                        column: x => x.VueloId,
                        principalTable: "Vuelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistorialEstados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VueloId = table.Column<Guid>(type: "uuid", nullable: false),
                    EstadoAnterior = table.Column<int>(type: "integer", nullable: false),
                    EstadoNuevo = table.Column<int>(type: "integer", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioResponsable = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialEstados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialEstados_Vuelos_VueloId",
                        column: x => x.VueloId,
                        principalTable: "Vuelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seguimientos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    VueloId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seguimientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seguimientos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Seguimientos_Vuelos_VueloId",
                        column: x => x.VueloId,
                        principalTable: "Vuelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aerolineas_Codigo",
                table: "Aerolineas",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistorialCambiosOperativos_VueloId",
                table: "HistorialCambiosOperativos",
                column: "VueloId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialEstados_VueloId",
                table: "HistorialEstados",
                column: "VueloId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_UsuarioId",
                table: "Notificaciones",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UsuarioId",
                table: "RefreshTokens",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Seguimientos_UsuarioId",
                table: "Seguimientos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Seguimientos_VueloId",
                table: "Seguimientos",
                column: "VueloId");

            migrationBuilder.CreateIndex(
                name: "IX_Vuelos_Aerolinea",
                table: "Vuelos",
                column: "Aerolinea");

            migrationBuilder.CreateIndex(
                name: "IX_Vuelos_Destino",
                table: "Vuelos",
                column: "Destino");

            migrationBuilder.CreateIndex(
                name: "IX_Vuelos_Origen",
                table: "Vuelos",
                column: "Origen");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistorialCambiosOperativos");

            migrationBuilder.DropTable(
                name: "HistorialEstados");

            migrationBuilder.DropTable(
                name: "LogAuditorias");

            migrationBuilder.DropTable(
                name: "Notificaciones");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Seguimientos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Vuelos");

            migrationBuilder.DropTable(
                name: "Aerolineas");

            migrationBuilder.DropTable(
                name: "Aeropuertos");
        }
    }
}
