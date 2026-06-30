using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Waylan_Origin.Migrations
{
    /// <inheritdoc />
    public partial class DbsetNotas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductoNotas_Nota_Notasid",
                table: "ProductoNotas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Nota",
                table: "Nota");

            migrationBuilder.RenameTable(
                name: "Nota",
                newName: "Notas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notas",
                table: "Notas",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductoNotas_Notas_Notasid",
                table: "ProductoNotas",
                column: "Notasid",
                principalTable: "Notas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductoNotas_Notas_Notasid",
                table: "ProductoNotas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notas",
                table: "Notas");

            migrationBuilder.RenameTable(
                name: "Notas",
                newName: "Nota");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Nota",
                table: "Nota",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductoNotas_Nota_Notasid",
                table: "ProductoNotas",
                column: "Notasid",
                principalTable: "Nota",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
