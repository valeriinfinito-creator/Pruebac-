using System;
using DeportivoApp.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeportivoApp.Migrations
{
    [DbContext(typeof(MySqlDBContext))]
    [Migration("20260428120000_UpdateEspaciosCatalog")]
    public partial class UpdateEspaciosCatalog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Espacios_Usuarios_UsuarioId",
                table: "Espacios");

            migrationBuilder.DropIndex(
                name: "IX_Espacios_UsuarioId",
                table: "Espacios");

            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "Espacios");

            migrationBuilder.DropColumn(
                name: "FechaInicio",
                table: "Espacios");

            migrationBuilder.DropColumn(
                name: "HoraFin",
                table: "Espacios");

            migrationBuilder.DropColumn(
                name: "HoraInicio",
                table: "Espacios");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Espacios");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Espacios",
                type: "varchar(191)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Capacidad",
                table: "Espacios",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Espacios",
                type: "varchar(191)",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Espacios_Nombre_Tipo",
                table: "Espacios",
                columns: new[] { "Nombre", "Tipo" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Espacios_Nombre_Tipo",
                table: "Espacios");

            migrationBuilder.DropColumn(
                name: "Capacidad",
                table: "Espacios");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Espacios");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Espacios",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(191)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha",
                table: "Espacios",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "FechaInicio",
                table: "Espacios",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HoraFin",
                table: "Espacios",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HoraInicio",
                table: "Espacios",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Espacios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Espacios_UsuarioId",
                table: "Espacios",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Espacios_Usuarios_UsuarioId",
                table: "Espacios",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
