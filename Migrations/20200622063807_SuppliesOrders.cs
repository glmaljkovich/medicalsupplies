using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace ArqNetCore.Migrations
{
    public partial class SuppliesOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupplyTypes",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SuppliesOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<string>(nullable: true),
                    AreaId = table.Column<string>(maxLength: 32, nullable: true),
                    OrganizationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuppliesOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuppliesOrders_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuppliesOrders_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuppliesOrders_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupplyTypeAttributes",
                columns: table => new
                {
                    SupplyTypeId = table.Column<string>(maxLength: 32, nullable: false),
                    AttributeName = table.Column<string>(maxLength: 32, nullable: false),
                    AttributeDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyTypeAttributes", x => new { x.SupplyTypeId, x.AttributeName });
                    table.ForeignKey(
                        name: "FK_SupplyTypeAttributes_SupplyTypes_SupplyTypeId",
                        column: x => x.SupplyTypeId,
                        principalTable: "SupplyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Supplies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    SupplyTypeId = table.Column<string>(nullable: true),
                    SuppliesOrderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supplies_SuppliesOrders_SuppliesOrderId",
                        column: x => x.SuppliesOrderId,
                        principalTable: "SuppliesOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Supplies_SupplyTypes_SupplyTypeId",
                        column: x => x.SupplyTypeId,
                        principalTable: "SupplyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupplyAttributes",
                columns: table => new
                {
                    SupplyId = table.Column<int>(nullable: false),
                    AttributeName = table.Column<string>(maxLength: 32, nullable: false),
                    AttributeValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyAttributes", x => new { x.SupplyId, x.AttributeName });
                    table.ForeignKey(
                        name: "FK_SupplyAttributes_Supplies_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_SuppliesOrderId",
                table: "Supplies",
                column: "SuppliesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_SupplyTypeId",
                table: "Supplies",
                column: "SupplyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SuppliesOrders_AccountId",
                table: "SuppliesOrders",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SuppliesOrders_AreaId",
                table: "SuppliesOrders",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_SuppliesOrders_OrganizationId",
                table: "SuppliesOrders",
                column: "OrganizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplyAttributes");

            migrationBuilder.DropTable(
                name: "SupplyTypeAttributes");

            migrationBuilder.DropTable(
                name: "Supplies");

            migrationBuilder.DropTable(
                name: "SuppliesOrders");

            migrationBuilder.DropTable(
                name: "SupplyTypes");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
