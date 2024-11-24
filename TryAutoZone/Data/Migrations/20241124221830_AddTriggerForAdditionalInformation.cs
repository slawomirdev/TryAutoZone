using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TryAutoZone.Data.Migrations
{
    public partial class AddTriggerForAdditionalInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        CREATE TRIGGER SetDefaultAdditionalInformation
        ON Reservations
        AFTER INSERT
        AS
        BEGIN
            UPDATE Reservations
            SET AdditionalInformation = 'No additional information provided'
            WHERE AdditionalInformation IS NULL OR AdditionalInformation = ''
            AND Id IN (SELECT Id FROM INSERTED);
        END;
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS SetDefaultAdditionalInformation;");
        }
    }
}
