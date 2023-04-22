using Microsoft.EntityFrameworkCore.Migrations;
using Returns.Logic.Utils;

#nullable disable

namespace Returns.Logic.Migrations
{
    /// <inheritdoc />
    public partial class InitData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(EmbeddedResourceReader.ReadAsync("InitData.CreateImportTables.sql").Result);
            migrationBuilder.Sql(EmbeddedResourceReader.ReadAsync("InitData.InsertIntoImportTables.sql").Result);

            migrationBuilder.Sql(EmbeddedResourceReader.ReadAsync("InitData.InsertIntoReturnAvailabilities.sql").Result);

            migrationBuilder.Sql(EmbeddedResourceReader.ReadAsync("InitData.InsertIntoFeeConfigurationGroups.sql").Result);
            migrationBuilder.Sql(EmbeddedResourceReader.ReadAsync("InitData.InsertIntoFeeConfigurations.sql").Result);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(EmbeddedResourceReader.ReadAsync("InitData.DeleteFeeConfigurationGroups.sql").Result);

            migrationBuilder.Sql(EmbeddedResourceReader.ReadAsync("InitData.DeleteReturnAvailabilities.sql").Result);
        }
    }
}
