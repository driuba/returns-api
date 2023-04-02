using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Returns.Logic.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCollation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserModified",
                table: "Returns",
                type: "TEXT",
                maxLength: 30,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserCreated",
                table: "Returns",
                type: "TEXT",
                maxLength: 30,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "RmaNumber",
                table: "Returns",
                type: "TEXT",
                maxLength: 20,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Returns",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryPointId",
                table: "Returns",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "Returns",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "Returns",
                type: "TEXT",
                maxLength: 3,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "Returns",
                type: "TEXT",
                maxLength: 3,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "UserModified",
                table: "ReturnLines",
                type: "TEXT",
                maxLength: 30,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserCreated",
                table: "ReturnLines",
                type: "TEXT",
                maxLength: 30,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "ReturnLines",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "NoteReturn",
                table: "ReturnLines",
                type: "TEXT",
                maxLength: 500,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NoteResponse",
                table: "ReturnLines",
                type: "TEXT",
                maxLength: 500,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNumberReturn",
                table: "ReturnLines",
                type: "TEXT",
                maxLength: 20,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNumberPurchase",
                table: "ReturnLines",
                type: "TEXT",
                maxLength: 20,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserModified",
                table: "ReturnLineDevices",
                type: "TEXT",
                maxLength: 30,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserCreated",
                table: "ReturnLineDevices",
                type: "TEXT",
                maxLength: 30,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "ReturnLineDevices",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "UserModified",
                table: "ReturnLineAttachments",
                type: "TEXT",
                maxLength: 30,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserCreated",
                table: "ReturnLineAttachments",
                type: "TEXT",
                maxLength: 30,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "ReturnLineAttachments",
                type: "TEXT",
                maxLength: 1000,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ReturnLineAttachments",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "UserModified",
                table: "ReturnFees",
                type: "TEXT",
                maxLength: 30,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserCreated",
                table: "ReturnFees",
                type: "TEXT",
                maxLength: 30,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "ReturnFees",
                type: "TEXT",
                maxLength: 20,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CountryId",
                table: "ReturnAvailabilities",
                type: "TEXT",
                maxLength: 2,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "ReturnAvailabilities",
                type: "TEXT",
                maxLength: 3,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "UserModified",
                table: "FeeConfigurations",
                type: "TEXT",
                maxLength: 30,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserCreated",
                table: "FeeConfigurations",
                type: "TEXT",
                maxLength: 30,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "FeeConfigurations",
                type: "TEXT",
                maxLength: 20,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CountryId",
                table: "FeeConfigurations",
                type: "TEXT",
                maxLength: 2,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "FeeConfigurationGroups",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "FeeConfigurationGroups",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "FeeConfigurationGroups",
                type: "TEXT",
                maxLength: 3,
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserModified",
                table: "Returns",
                type: "TEXT",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "UserCreated",
                table: "Returns",
                type: "TEXT",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "RmaNumber",
                table: "Returns",
                type: "TEXT",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Returns",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryPointId",
                table: "Returns",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "Returns",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "Returns",
                type: "TEXT",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 3,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "Returns",
                type: "TEXT",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 3,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "UserModified",
                table: "ReturnLines",
                type: "TEXT",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "UserCreated",
                table: "ReturnLines",
                type: "TEXT",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "ReturnLines",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "NoteReturn",
                table: "ReturnLines",
                type: "TEXT",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "NoteResponse",
                table: "ReturnLines",
                type: "TEXT",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNumberReturn",
                table: "ReturnLines",
                type: "TEXT",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNumberPurchase",
                table: "ReturnLines",
                type: "TEXT",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "UserModified",
                table: "ReturnLineDevices",
                type: "TEXT",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "UserCreated",
                table: "ReturnLineDevices",
                type: "TEXT",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "ReturnLineDevices",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "UserModified",
                table: "ReturnLineAttachments",
                type: "TEXT",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "UserCreated",
                table: "ReturnLineAttachments",
                type: "TEXT",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "ReturnLineAttachments",
                type: "TEXT",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 1000,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ReturnLineAttachments",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "UserModified",
                table: "ReturnFees",
                type: "TEXT",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "UserCreated",
                table: "ReturnFees",
                type: "TEXT",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "ReturnFees",
                type: "TEXT",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "CountryId",
                table: "ReturnAvailabilities",
                type: "TEXT",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 2,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "ReturnAvailabilities",
                type: "TEXT",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 3,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "UserModified",
                table: "FeeConfigurations",
                type: "TEXT",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "UserCreated",
                table: "FeeConfigurations",
                type: "TEXT",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "FeeConfigurations",
                type: "TEXT",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "CountryId",
                table: "FeeConfigurations",
                type: "TEXT",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 2,
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "FeeConfigurationGroups",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "FeeConfigurationGroups",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "FeeConfigurationGroups",
                type: "TEXT",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 3,
                oldCollation: "NOCASE");
        }
    }
}
