using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiroCalendar.Api.Migrations;

/// <inheritdoc />
public partial class AddCalendarKeys : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "Guid",
            table: "Records",
            type: "TEXT",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.CreateTable(
            name: "CalendarAccessKeys",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                BiroAccountId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CalendarAccessKeys", x => x.Id);
                table.ForeignKey(
                    name: "FK_CalendarAccessKeys_BiroAccounts_BiroAccountId",
                    column: x => x.BiroAccountId,
                    principalTable: "BiroAccounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_CalendarAccessKeys_BiroAccountId",
            table: "CalendarAccessKeys",
            column: "BiroAccountId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "CalendarAccessKeys");

        migrationBuilder.DropColumn(
            name: "Guid",
            table: "Records");
    }
}
