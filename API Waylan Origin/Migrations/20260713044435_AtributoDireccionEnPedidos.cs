using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Waylan_Origin.Migrations
{
    /// <inheritdoc />
    public partial class AtributoDireccionEnPedidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "pedidos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "pedidos");
        }
    }
}
