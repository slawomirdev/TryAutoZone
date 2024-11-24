using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TryAutoZone.Data.Migrations
{
    public partial class AddLogsAndTriggers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Dodaj tabelę Logs
            migrationBuilder.Sql(@"
        CREATE TABLE Logs (
            Id INT IDENTITY(1,1) PRIMARY KEY,
            Action NVARCHAR(50) NOT NULL,
            TableName NVARCHAR(50) NOT NULL,
            Timestamp DATETIME DEFAULT GETDATE(),
            Description NVARCHAR(MAX)
        );
    ");

            // Trigger dla INSERT
            migrationBuilder.Sql(@"
        CREATE TRIGGER LogInsertOnReservations
        ON Reservations
        AFTER INSERT
        AS
        BEGIN
            INSERT INTO Logs (Action, TableName, Description)
            SELECT 'INSERT', 'Reservations', 
                   'New reservation added for car ID: ' + CAST(CarId AS NVARCHAR)
            FROM INSERTED;
        END;
    ");

            // Trigger dla UPDATE
            migrationBuilder.Sql(@"
        CREATE TRIGGER LogUpdateOnReservations
        ON Reservations
        AFTER UPDATE
        AS
        BEGIN
            INSERT INTO Logs (Action, TableName, Description)
            SELECT 'UPDATE', 'Reservations', 
                   'Reservation updated for ID: ' + CAST(Id AS NVARCHAR)
            FROM INSERTED;
        END;
    ");

            // Trigger dla DELETE
            migrationBuilder.Sql(@"
        CREATE TRIGGER LogDeleteOnReservations
        ON Reservations
        AFTER DELETE
        AS
        BEGIN
            INSERT INTO Logs (Action, TableName, Description)
            SELECT 'DELETE', 'Reservations', 
                   'Reservation deleted for ID: ' + CAST(Id AS NVARCHAR)
            FROM DELETED;
        END;
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER LogInsertOnReservations;");
            migrationBuilder.Sql("DROP TRIGGER LogUpdateOnReservations;");
            migrationBuilder.Sql("DROP TRIGGER LogDeleteOnReservations;");
            migrationBuilder.Sql("DROP TABLE Logs;");
        }
    }
}
