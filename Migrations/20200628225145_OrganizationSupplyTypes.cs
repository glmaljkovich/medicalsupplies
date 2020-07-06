using Microsoft.EntityFrameworkCore.Migrations;

namespace ArqNetCore.Migrations
{
    public partial class OrganizationSupplyTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "SuppliesOrders",
                maxLength: 32,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrganizationSupplyTypes",
                columns: table => new
                {
                    OrganizationId = table.Column<int>(nullable: false),
                    SupplyTypeId = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationSupplyTypes", x => new { x.SupplyTypeId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_OrganizationSupplyTypes_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationSupplyTypes_SupplyTypes_SupplyTypeId",
                        column: x => x.SupplyTypeId,
                        principalTable: "SupplyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Insumos Basicos" });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Insumos mecanicos" });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Medicamentos" });

            migrationBuilder.InsertData(
                table: "OrganizationSupplyTypes",
                columns: new[] { "SupplyTypeId", "OrganizationId" },
                values: new object[,]
                {
                    { "BARBIJO", 1 },
                    { "GUANTE", 1 },
                    { "MASCARA_PROTECTORA", 1 },
                    { "RESPIRADOR", 2 },
                    { "MEDICAMENTO", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationSupplyTypes_OrganizationId",
                table: "OrganizationSupplyTypes",
                column: "OrganizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationSupplyTypes");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SuppliesOrders");
        }
    }
}
