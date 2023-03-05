using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Returns.Logic.Migrations
{
    /// <inheritdoc />
    public partial class DataModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeeConfigurationGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    DelayDays = table.Column<int>(type: "INTEGER", nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeConfigurationGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReturnAvailabilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    CountryId = table.Column<string>(type: "TEXT", maxLength: 2, nullable: true),
                    Days = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnAvailabilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Returns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    CurrencyCode = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    CustomerId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    DeliveryPointId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    LabelCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Number = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    RmaNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    State = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserCreated = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    UserModified = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Returns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeeConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CountryId = table.Column<string>(type: "TEXT", maxLength: 2, nullable: true),
                    CustomerId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    FeeConfigurationGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    ValueMinimum = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: true),
                    ValueType = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserCreated = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    UserModified = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeConfigurations", x => x.Id);
                    table.CheckConstraint("CK_FeeConfigurations_CountryId_CustomerId", "[CountryId] IS NULL OR [CustomerId] IS NULL");
                    table.ForeignKey(
                        name: "FK_FeeConfigurations_FeeConfigurationGroups_FeeConfigurationGroupId",
                        column: x => x.FeeConfigurationGroupId,
                        principalTable: "FeeConfigurationGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvoiceNumberPurchase = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    InvoiceNumberReturn = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    NoteReturn = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    NoteResponse = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    PriceUnit = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    ProductId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    ProductType = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    ReturnId = table.Column<int>(type: "INTEGER", nullable: false),
                    State = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserCreated = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    UserModified = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnLines_Returns_ReturnId",
                        column: x => x.ReturnId,
                        principalTable: "Returns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnFees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FeeConfigurationId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    ReturnId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReturnLineId = table.Column<int>(type: "INTEGER", nullable: true),
                    Value = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserCreated = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    UserModified = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnFees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnFees_FeeConfigurations_FeeConfigurationId",
                        column: x => x.FeeConfigurationId,
                        principalTable: "FeeConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReturnFees_ReturnLines_ReturnLineId",
                        column: x => x.ReturnLineId,
                        principalTable: "ReturnLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnFees_Returns_ReturnId",
                        column: x => x.ReturnId,
                        principalTable: "Returns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnLineAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    ReturnLineId = table.Column<int>(type: "INTEGER", nullable: false),
                    StorageEntryId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Url = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserCreated = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    UserModified = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnLineAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnLineAttachments_ReturnLines_ReturnLineId",
                        column: x => x.ReturnLineId,
                        principalTable: "ReturnLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnLineDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReturnLineId = table.Column<int>(type: "INTEGER", nullable: false),
                    SerialNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserCreated = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    UserModified = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true)
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
                name: "IX_FeeConfigurations_CountryId_CustomerId_FeeConfigurationGroupId",
                table: "FeeConfigurations",
                columns: new[] { "CountryId", "CustomerId", "FeeConfigurationGroupId" },
                unique: true,
                filter: "[Deleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_FeeConfigurations_FeeConfigurationGroupId",
                table: "FeeConfigurations",
                column: "FeeConfigurationGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnAvailabilities_CompanyId_CountryId",
                table: "ReturnAvailabilities",
                columns: new[] { "CompanyId", "CountryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReturnFees_FeeConfigurationId",
                table: "ReturnFees",
                column: "FeeConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnFees_ReturnId",
                table: "ReturnFees",
                column: "ReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnFees_ReturnLineId",
                table: "ReturnFees",
                column: "ReturnLineId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnLineAttachments_ReturnLineId",
                table: "ReturnLineAttachments",
                column: "ReturnLineId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnLineDevices_ReturnLineId",
                table: "ReturnLineDevices",
                column: "ReturnLineId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReturnLineDevices_SerialNumber",
                table: "ReturnLineDevices",
                column: "SerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnLines_InvoiceNumberPurchase_ProductId",
                table: "ReturnLines",
                columns: new[] { "InvoiceNumberPurchase", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_ReturnLines_ReturnId",
                table: "ReturnLines",
                column: "ReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_CompanyId_CustomerId",
                table: "Returns",
                columns: new[] { "CompanyId", "CustomerId" });

            migrationBuilder.CreateIndex(
                name: "IX_Returns_CompanyId_Number",
                table: "Returns",
                columns: new[] { "CompanyId", "Number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReturnAvailabilities");

            migrationBuilder.DropTable(
                name: "ReturnFees");

            migrationBuilder.DropTable(
                name: "ReturnLineAttachments");

            migrationBuilder.DropTable(
                name: "ReturnLineDevices");

            migrationBuilder.DropTable(
                name: "FeeConfigurations");

            migrationBuilder.DropTable(
                name: "ReturnLines");

            migrationBuilder.DropTable(
                name: "FeeConfigurationGroups");

            migrationBuilder.DropTable(
                name: "Returns");
        }
    }
}
