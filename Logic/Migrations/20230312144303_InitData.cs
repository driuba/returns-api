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
            migrationBuilder.Sql(EmbeddedResourceReader.Read("InitData.CreateImportTables.sql").Result);
            migrationBuilder.Sql(EmbeddedResourceReader.Read("InitData.InsertIntoImportTables.sql").Result);

            migrationBuilder.Sql(EmbeddedResourceReader.Read("InitData.InsertIntoReturnAvailabilities.sql").Result);

            migrationBuilder.Sql(EmbeddedResourceReader.Read("InitData.InsertIntoFeeConfigurationGroups.sql").Result);
            migrationBuilder.Sql(EmbeddedResourceReader.Read("InitData.InsertIntoFeeConfigurations.sql").Result);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(EmbeddedResourceReader.Read("InitData.DeleteFeeConfigurationGroups.sql").Result);

            migrationBuilder.Sql(EmbeddedResourceReader.Read("InitData.DeleteReturnAvailabilities.sql").Result);
        }
    }
}
