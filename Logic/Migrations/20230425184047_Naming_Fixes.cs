using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Returns.Logic.Migrations
{
    /// <inheritdoc />
    public partial class Naming_Fixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FeeConfigurations_RegionId_CustomerId_FeeConfigurationGroupId",
                table: "FeeConfigurations");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FeeConfigurations_CountryId_CustomerId",
                table: "FeeConfigurations");

            migrationBuilder.CreateIndex(
                name: "IX_FeeConfigurations_CustomerId_FeeConfigurationGroupId_RegionId",
                table: "FeeConfigurations",
                columns: new[] { "CustomerId", "FeeConfigurationGroupId", "RegionId" },
                unique: true,
                filter: "[Deleted] = 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FeeConfigurations_RegionId_CustomerId",
                table: "FeeConfigurations",
                sql: "[RegionId] IS NULL OR [CustomerId] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FeeConfigurations_CustomerId_FeeConfigurationGroupId_RegionId",
                table: "FeeConfigurations");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FeeConfigurations_RegionId_CustomerId",
                table: "FeeConfigurations");

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
    }
}
