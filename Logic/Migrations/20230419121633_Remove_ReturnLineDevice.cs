using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Returns.Logic.Migrations
{
    /// <inheritdoc />
    public partial class Remove_ReturnLineDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReturnLineDevices");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "Returns");

            migrationBuilder.DropColumn(
                name: "StorageEntryId",
                table: "ReturnLineAttachments");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "ReturnLineAttachments");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "ReturnAvailabilities",
                newName: "RegionId");

            migrationBuilder.RenameIndex(
                name: "IX_ReturnAvailabilities_CompanyId_CountryId",
                table: "ReturnAvailabilities",
                newName: "IX_ReturnAvailabilities_CompanyId_RegionId");

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "ReturnLines",
                type: "TEXT",
                maxLength: 50,
                nullable: true,
                collation: "NOCASE");

            migrationBuilder.AddColumn<Guid>(
                name: "StorageId",
                table: "ReturnLineAttachments",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "RegionId",
                table: "ReturnAvailabilities",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 2,
                oldNullable: true,
                oldCollation: "NOCASE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "ReturnLines");

            migrationBuilder.DropColumn(
                name: "StorageId",
                table: "ReturnLineAttachments");

            migrationBuilder.RenameColumn(
                name: "RegionId",
                table: "ReturnAvailabilities",
                newName: "CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_ReturnAvailabilities_CompanyId_RegionId",
                table: "ReturnAvailabilities",
                newName: "IX_ReturnAvailabilities_CompanyId_CountryId");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "Returns",
                type: "TEXT",
                maxLength: 3,
                nullable: false,
                defaultValue: "",
                collation: "NOCASE");

            migrationBuilder.AddColumn<Guid>(
                name: "StorageEntryId",
                table: "ReturnLineAttachments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "ReturnLineAttachments",
                type: "TEXT",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                collation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "CountryId",
                table: "ReturnAvailabilities",
                type: "TEXT",
                maxLength: 2,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ReturnLineDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReturnLineId = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SerialNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, collation: "NOCASE"),
                    UserCreated = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false, collation: "NOCASE"),
                    UserModified = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnLineDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnLineDevices_ReturnLines_ReturnLineId",
                        column: x => x.ReturnLineId,
                        principalTable: "ReturnLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReturnLineDevices_ReturnLineId",
                table: "ReturnLineDevices",
                column: "ReturnLineId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReturnLineDevices_SerialNumber",
                table: "ReturnLineDevices",
                column: "SerialNumber");
        }
    }
}
