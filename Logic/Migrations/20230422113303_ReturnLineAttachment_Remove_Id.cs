using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Returns.Logic.Migrations
{
    /// <inheritdoc />
    public partial class ReturnLineAttachment_Remove_Id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReturnLineAttachments",
                table: "ReturnLineAttachments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ReturnLineAttachments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReturnLineAttachments",
                table: "ReturnLineAttachments",
                column: "StorageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReturnLineAttachments",
                table: "ReturnLineAttachments");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ReturnLineAttachments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReturnLineAttachments",
                table: "ReturnLineAttachments",
                column: "Id");
        }
    }
}
