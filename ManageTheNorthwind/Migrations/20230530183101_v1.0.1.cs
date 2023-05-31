using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManageTheNorthwind.Migrations
{
    public partial class v101 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Details_Orders",
                table: "OrderDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Details_Orders",
                table: "OrderDetails",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Details_Orders",
                table: "OrderDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Details_Orders",
                table: "OrderDetails",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID");
        }
    }
}
