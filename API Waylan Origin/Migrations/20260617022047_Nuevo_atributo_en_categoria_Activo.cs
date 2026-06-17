using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Waylan_Origin.Migrations
{
    /// <inheritdoc />
    public partial class Nuevo_atributo_en_categoria_Activo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "categorias",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "categorias");
        }
    }
}
