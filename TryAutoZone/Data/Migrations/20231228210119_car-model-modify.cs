using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TryAutoZone.Data.Migrations
{
    public partial class carmodelmodify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CO2Emission",
                table: "Car",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EngineCapacity",
                table: "Car",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EngineType",
                table: "Car",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "FuelConsumption",
                table: "Car",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Gearbox",
                table: "Car",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "HorsePower",
                table: "Car",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CO2Emission",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "EngineCapacity",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "EngineType",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "FuelConsumption",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "Gearbox",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "HorsePower",
                table: "Car");
        }
    }
}
