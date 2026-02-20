using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaCaixaPostalIA.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CaixasStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaixasStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientesStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientesStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CobrancasStatus",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Nome = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CobrancasStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormaPagamentos",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Nome = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormaPagamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Socios",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    Numero = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Socios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdClienteStatus = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: true),
                    Telefone = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clientes_ClientesStatus_IdClienteStatus",
                        column: x => x.IdClienteStatus,
                        principalTable: "ClientesStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CaixasPostais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdSocio = table.Column<short>(type: "smallint", nullable: false),
                    IdCliente = table.Column<int>(type: "integer", nullable: false),
                    IdStatusCaixa = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Codigo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NomeEmpresa = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CpfCnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    DataAluguel = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "CURRENT_DATE"),
                    DiaVencimento = table.Column<int>(type: "integer", nullable: false),
                    ValorMensal = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaixasPostais", x => x.Id);
                    table.CheckConstraint("CK_CaixasPostais_DiaVencimento", "\"DiaVencimento\" BETWEEN 1 AND 31");
                    table.ForeignKey(
                        name: "FK_CaixasPostais_CaixasStatus_IdStatusCaixa",
                        column: x => x.IdStatusCaixa,
                        principalTable: "CaixasStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaixasPostais_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaixasPostais_Socios_IdSocio",
                        column: x => x.IdSocio,
                        principalTable: "Socios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cobrancas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCaixaPostal = table.Column<int>(type: "integer", nullable: false),
                    IdStatusCobranca = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    Valor = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DataVencimento = table.Column<DateTime>(type: "date", nullable: false),
                    DataLiquidacao = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cobrancas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cobrancas_CaixasPostais_IdCaixaPostal",
                        column: x => x.IdCaixaPostal,
                        principalTable: "CaixasPostais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cobrancas_CobrancasStatus_IdStatusCobranca",
                        column: x => x.IdStatusCobranca,
                        principalTable: "CobrancasStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistoricoPagamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCobranca = table.Column<int>(type: "integer", nullable: false),
                    IdFormaPagamento = table.Column<short>(type: "smallint", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "CURRENT_DATE"),
                    ValorPago = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Observacao = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoPagamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoPagamentos_Cobrancas_IdCobranca",
                        column: x => x.IdCobranca,
                        principalTable: "Cobrancas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistoricoPagamentos_FormaPagamentos_IdFormaPagamento",
                        column: x => x.IdFormaPagamento,
                        principalTable: "FormaPagamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "CaixasStatus",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Ativa" },
                    { 2, "Bloqueada" },
                    { 3, "Cancelada" }
                });

            migrationBuilder.InsertData(
                table: "ClientesStatus",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Ativo" },
                    { 2, "Inativo" },
                    { 3, "Inadimplente" }
                });

            migrationBuilder.InsertData(
                table: "CobrancasStatus",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { (short)1, "Pendente" },
                    { (short)2, "Pago" },
                    { (short)3, "Atrasado" },
                    { (short)4, "Cancelado" }
                });

            migrationBuilder.InsertData(
                table: "FormaPagamentos",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { (short)1, "Dinheiro" },
                    { (short)2, "PIX" },
                    { (short)3, "Boleto" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaixasPostais_IdCliente",
                table: "CaixasPostais",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_CaixasPostais_IdSocio",
                table: "CaixasPostais",
                column: "IdSocio");

            migrationBuilder.CreateIndex(
                name: "IX_CaixasPostais_IdStatusCaixa",
                table: "CaixasPostais",
                column: "IdStatusCaixa");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_IdClienteStatus",
                table: "Clientes",
                column: "IdClienteStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Cobrancas_IdCaixaPostal",
                table: "Cobrancas",
                column: "IdCaixaPostal");

            migrationBuilder.CreateIndex(
                name: "IX_Cobrancas_IdStatusCobranca",
                table: "Cobrancas",
                column: "IdStatusCobranca");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoPagamentos_IdCobranca",
                table: "HistoricoPagamentos",
                column: "IdCobranca");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoPagamentos_IdFormaPagamento",
                table: "HistoricoPagamentos",
                column: "IdFormaPagamento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricoPagamentos");

            migrationBuilder.DropTable(
                name: "Cobrancas");

            migrationBuilder.DropTable(
                name: "FormaPagamentos");

            migrationBuilder.DropTable(
                name: "CaixasPostais");

            migrationBuilder.DropTable(
                name: "CobrancasStatus");

            migrationBuilder.DropTable(
                name: "CaixasStatus");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Socios");

            migrationBuilder.DropTable(
                name: "ClientesStatus");
        }
    }
}
