using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Waylan_Origin.Migrations
{
    /// <inheritdoc />
    public partial class CamposDeWompiEnEntidadPedidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EstadoPago",
                table: "pedidos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaPago",
                table: "pedidos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WompiTransactionId",
                table: "pedidos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoPago",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "FechaPago",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "WompiTransactionId",
                table: "pedidos");
        }
    }
}
