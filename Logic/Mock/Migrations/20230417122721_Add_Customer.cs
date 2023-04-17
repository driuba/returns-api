using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Returns.Logic.Mock.Migrations
{
    /// <inheritdoc />
    public partial class Add_Customer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CompanyId = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false, collation: "NOCASE"),
                    Id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false, collation: "NOCASE"),
                    CountryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, collation: "NOCASE"),
                    ParentId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => new { x.CompanyId, x.Id });
                    table.ForeignKey(
                        name: "FK_Customers_Customers_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customers_Regions_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CountryId",
                table: "Customers",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ParentId",
                table: "Customers",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
