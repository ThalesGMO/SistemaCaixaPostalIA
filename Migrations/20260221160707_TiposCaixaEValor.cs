using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaCaixaPostalIA.Migrations
{
    /// <inheritdoc />
    public partial class TiposCaixaEValor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ValorMensal",
                table: "CaixasPostais",
                newName: "Valor");

            migrationBuilder.InsertData(
                table: "TiposCaixa",
                columns: new[] { "Id", "Nome" },
                values: new object[] { 3, "Mensalidade" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TiposCaixa",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameColumn(
                name: "Valor",
                table: "CaixasPostais",
                newName: "ValorMensal");
        }
    }
}
