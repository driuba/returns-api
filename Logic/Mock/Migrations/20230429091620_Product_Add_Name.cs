using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Returns.Logic.Mock.Migrations
{
    /// <inheritdoc />
    public partial class Product_Add_Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Products",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                collation: "NOCASE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Products");
        }
    }
}
