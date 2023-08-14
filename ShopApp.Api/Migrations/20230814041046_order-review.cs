using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class orderreview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Reviews",
                newName: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_OrderDetailId",
                table: "Reviews",
                column: "OrderDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_OrderDetail_OrderDetailId",
                table: "Reviews",
                column: "OrderDetailId",
                principalTable: "OrderDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_OrderDetail_OrderDetailId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_OrderDetailId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "OrderDetailId",
                table: "Reviews",
                newName: "ProductId");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
