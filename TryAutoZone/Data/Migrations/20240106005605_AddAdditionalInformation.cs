using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TryAutoZone.Data.Migrations
{
    public partial class AddAdditionalInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalInformation",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalInformation",
                table: "Reservations");
        }
    }
}
