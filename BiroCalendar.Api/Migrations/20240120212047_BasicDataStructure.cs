using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiroCalendar.Api.Migrations;

/// <inheritdoc />
public partial class BasicDataStructure : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Accounts",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                EmailAddress = table.Column<string>(type: "TEXT", nullable: false),
                PasswordHash = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Accounts", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "BiroAccounts",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                AccountName = table.Column<string>(type: "TEXT", nullable: false),
                AccountPassword = table.Column<string>(type: "TEXT", nullable: false),
                ServiceUrl = table.Column<string>(type: "TEXT", nullable: false),
                Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                LastAccessed = table.Column<DateTime>(type: "TEXT", nullable: true),
                AccountId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BiroAccounts", x => x.Id);
                table.ForeignKey(
                    name: "FK_BiroAccounts_Accounts_AccountId",
                    column: x => x.AccountId,
                    principalTable: "Accounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Records",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                ClassName = table.Column<string>(type: "TEXT", nullable: false),
                TaskName = table.Column<string>(type: "TEXT", nullable: false),
                TaskDueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                Outdated = table.Column<bool>(type: "INTEGER", nullable: false),
                FetchedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                OutdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                BiroAccountId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Records", x => x.Id);
                table.ForeignKey(
                    name: "FK_Records_BiroAccounts_BiroAccountId",
                    column: x => x.BiroAccountId,
                    principalTable: "BiroAccounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Accounts_EmailAddress",
            table: "Accounts",
            column: "EmailAddress",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_BiroAccounts_AccountId",
            table: "BiroAccounts",
            column: "AccountId");

        migrationBuilder.CreateIndex(
            name: "IX_Records_BiroAccountId",
            table: "Records",
            column: "BiroAccountId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Records");

        migrationBuilder.DropTable(
            name: "BiroAccounts");

        migrationBuilder.DropTable(
            name: "Accounts");
    }
}
