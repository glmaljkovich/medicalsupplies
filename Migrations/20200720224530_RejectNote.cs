using Microsoft.EntityFrameworkCore.Migrations;

namespace ArqNetCore.Migrations
{
    public partial class RejectNote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "SuppliesOrders",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "SuppliesOrders");
        }
    }
}
