using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System.Security.Cryptography;
using System.Text;

namespace ArqNetCore.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    Enable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                name: "Users",
                columns: table => new
                {
                    Email = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    Locality = table.Column<string>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Email);
                    table.ForeignKey(
                        name: "FK_Users_Accounts_Email",
                        column: x => x.Email,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuppliesOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(nullable: true),
                    AreaId = table.Column<string>(maxLength: 32, nullable: true),
                    OrganizationId = table.Column<int>(nullable: true),
                    Status = table.Column<string>(maxLength: 32, nullable: true)
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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

            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Name", "Description" },
                values: new object[,]
                {
                    { "ATENCION_A_PACIENTES", "Atención de pacientes" },
                    { "TERAPIA_INTENSIVA", "Terapia Intensiva" },
                    { "TECNICOS", "Técnicos" }
                });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Insumos Basicos" },
                    { 2, "Insumos mecanicos" },
                    { 3, "Medicamentos" }
                });

            migrationBuilder.InsertData(
                table: "SupplyTypes",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { "MASCARA_PROTECTORA", "Máscara protectora" },
                    { "BARBIJO", "Barbijo" },
                    { "RESPIRADOR", "Respirador" },
                    { "GUANTE", "Guante" },
                    { "MEDICAMENTO", "Medicamento" }
                });

            migrationBuilder.InsertData(
                table: "OrganizationSupplyTypes",
                columns: new[] { "SupplyTypeId", "OrganizationId" },
                values: new object[,]
                {
                    { "MASCARA_PROTECTORA", 1 },
                    { "BARBIJO", 1 },
                    { "RESPIRADOR", 2 },
                    { "GUANTE", 1 },
                    { "MEDICAMENTO", 3 }
                });

            string adminEmail = "admin@medicalsupplies.org";
            string adminPassRaw = "123456";
            byte[] valueHash = null;
            byte[] valueSalt = null;
            using (var hmac = new HMACSHA512())
            {
                valueSalt = hmac.Key;
                byte[] encoded = Encoding.UTF8.GetBytes(adminPassRaw);
                valueHash = hmac.ComputeHash(encoded);
            }

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "PasswordHash", "PasswordSalt", "Enable" },
                values: new object[]{ adminEmail, valueHash, valueSalt, true });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Email", "FirstName", "IsAdmin" },
                values: new object[]{ adminEmail, "admin", true });

            migrationBuilder.InsertData(
                table: "SupplyTypeAttributes",
                columns: new[] { "SupplyTypeId", "AttributeName", "AttributeDescription" },
                values: new object[] { "MEDICAMENTO", "TIPO_MEDICAMENTO", "Tipo de medicamento" });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationSupplyTypes_OrganizationId",
                table: "OrganizationSupplyTypes",
                column: "OrganizationId");

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
                name: "OrganizationSupplyTypes");

            migrationBuilder.DropTable(
                name: "SupplyAttributes");

            migrationBuilder.DropTable(
                name: "SupplyTypeAttributes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Supplies");

            migrationBuilder.DropTable(
                name: "SuppliesOrders");

            migrationBuilder.DropTable(
                name: "SupplyTypes");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
