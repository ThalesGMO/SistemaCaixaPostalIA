using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaCaixaPostalIA.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarTipoCaixa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdTipoCaixa",
                table: "CaixasPostais",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "TiposCaixa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposCaixa", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TiposCaixa",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Anuidade" },
                    { 2, "Cortesia" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaixasPostais_IdTipoCaixa",
                table: "CaixasPostais",
                column: "IdTipoCaixa");

            migrationBuilder.AddForeignKey(
                name: "FK_CaixasPostais_TiposCaixa_IdTipoCaixa",
                table: "CaixasPostais",
                column: "IdTipoCaixa",
                principalTable: "TiposCaixa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaixasPostais_TiposCaixa_IdTipoCaixa",
                table: "CaixasPostais");

            migrationBuilder.DropTable(
                name: "TiposCaixa");

            migrationBuilder.DropIndex(
                name: "IX_CaixasPostais_IdTipoCaixa",
                table: "CaixasPostais");

            migrationBuilder.DropColumn(
                name: "IdTipoCaixa",
                table: "CaixasPostais");
        }
    }
}
