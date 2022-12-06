using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaStore.Web.Migrations
{
    public partial class Manga2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedido_EnderecoCliente_EnderecoId",
                table: "Pedido");

            migrationBuilder.CreateTable(
                name: "StatusPedido",
                columns: table => new
                {
                    Id = table.Column<int>(type: "Int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PedidoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusPedido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusPedido_Pedido_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatusPedido_PedidoId",
                table: "StatusPedido",
                column: "PedidoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedido_EnderecoCliente_EnderecoId",
                table: "Pedido",
                column: "EnderecoId",
                principalTable: "EnderecoCliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedido_EnderecoCliente_EnderecoId",
                table: "Pedido");

            migrationBuilder.DropTable(
                name: "StatusPedido");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedido_EnderecoCliente_EnderecoId",
                table: "Pedido",
                column: "EnderecoId",
                principalTable: "EnderecoCliente",
                principalColumn: "Id");
        }
    }
}
