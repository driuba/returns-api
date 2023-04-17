using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Returns.Logic.Migrations
{
    /// <inheritdoc />
    public partial class Update_FeeConfiguration_CountryId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FeeConfigurations_CountryId_CustomerId_FeeConfigurationGroupId",
                table: "FeeConfigurations");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FeeConfigurations_CountryId_CustomerId",
                table: "FeeConfigurations");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "FeeConfigurations");

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "FeeConfigurations",
                type: "INTEGER",
                maxLength: 2,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeeConfigurations_RegionId_CustomerId_FeeConfigurationGroupId",
                table: "FeeConfigurations",
                columns: new[] { "RegionId", "CustomerId", "FeeConfigurationGroupId" },
                unique: true,
                filter: "[Deleted] = 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FeeConfigurations_CountryId_CustomerId",
                table: "FeeConfigurations",
                sql: "[RegionId] IS NULL OR [CustomerId] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FeeConfigurations_RegionId_CustomerId_FeeConfigurationGroupId",
                table: "FeeConfigurations");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FeeConfigurations_CountryId_CustomerId",
                table: "FeeConfigurations");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "FeeConfigurations");

            migrationBuilder.AddColumn<string>(
                name: "CountryId",
                table: "FeeConfigurations",
                type: "TEXT",
                maxLength: 2,
                nullable: true,
                collation: "NOCASE");

            migrationBuilder.CreateIndex(
                name: "IX_FeeConfigurations_CountryId_CustomerId_FeeConfigurationGroupId",
                table: "FeeConfigurations",
                columns: new[] { "CountryId", "CustomerId", "FeeConfigurationGroupId" },
                unique: true,
                filter: "[Deleted] = 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FeeConfigurations_CountryId_CustomerId",
                table: "FeeConfigurations",
                sql: "[CountryId] IS NULL OR [CustomerId] IS NULL");
        }
    }
}
