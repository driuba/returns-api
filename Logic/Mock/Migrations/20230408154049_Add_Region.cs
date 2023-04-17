using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Returns.Logic.Mock.Migrations
{
    /// <inheritdoc />
    public partial class Add_Region : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, collation: "NOCASE"),
                    Type = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegionRegion",
                columns: table => new
                {
                    ChildrenId = table.Column<int>(type: "INTEGER", nullable: false),
                    ParentsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionRegion", x => new { x.ChildrenId, x.ParentsId });
                    table.ForeignKey(
                        name: "FK_RegionRegion_Regions_ChildrenId",
                        column: x => x.ChildrenId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegionRegion_Regions_ParentsId",
                        column: x => x.ParentsId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegionRegion_ParentsId",
                table: "RegionRegion",
                column: "ParentsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegionRegion");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
