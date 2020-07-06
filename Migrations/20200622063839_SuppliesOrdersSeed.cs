using Microsoft.EntityFrameworkCore.Migrations;

namespace ArqNetCore.Migrations
{
    public partial class SuppliesOrdersSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "SupplyTypeAttributes",
                columns: new[] { "SupplyTypeId", "AttributeName", "AttributeDescription" },
                values: new object[] { "MEDICAMENTO", "TIPO_MEDICAMENTO", "Tipo de medicamento" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Name",
                keyValue: "ATENCION_A_PACIENTES");

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Name",
                keyValue: "TECNICOS");

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Name",
                keyValue: "TERAPIA_INTENSIVA");

            migrationBuilder.DeleteData(
                table: "SupplyTypeAttributes",
                keyColumns: new[] { "SupplyTypeId", "AttributeName" },
                keyValues: new object[] { "MEDICAMENTO", "TIPO_MEDICAMENTO" });

            migrationBuilder.DeleteData(
                table: "SupplyTypes",
                keyColumn: "Id",
                keyValue: "BARBIJO");

            migrationBuilder.DeleteData(
                table: "SupplyTypes",
                keyColumn: "Id",
                keyValue: "GUANTE");

            migrationBuilder.DeleteData(
                table: "SupplyTypes",
                keyColumn: "Id",
                keyValue: "MASCARA_PROTECTORA");

            migrationBuilder.DeleteData(
                table: "SupplyTypes",
                keyColumn: "Id",
                keyValue: "RESPIRADOR");

            migrationBuilder.DeleteData(
                table: "SupplyTypes",
                keyColumn: "Id",
                keyValue: "MEDICAMENTO");
        }
    }
}
