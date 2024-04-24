using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AzureAdB2BApi.Migrations
{
    /// <inheritdoc />
    public partial class AddingMultiTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserInvitations",
                keyColumn: "Id",
                keyValue: new Guid("24ba9736-b7ae-411c-a516-f1f8cff0d251"));

            migrationBuilder.DeleteData(
                table: "UserInvitations",
                keyColumn: "Id",
                keyValue: new Guid("9911db18-e8ba-4b83-8c4f-2676e8fc8cfb"));

            migrationBuilder.CreateTable(
                name: "TenantInfo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Identifier = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantInfo", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "UserInvitations",
                columns: new[] { "Id", "CreatedBy", "CreatedTime", "CustomerId", "DelegatedUserManagementRole", "ExpiresTime", "InvitationCode" },
                values: new object[,]
                {
                    { new Guid("2c5dda4c-78a6-4f7f-a991-964a7f199943"), "system", new DateTimeOffset(new DateTime(2024, 4, 24, 12, 48, 56, 886, DateTimeKind.Unspecified).AddTicks(7150), new TimeSpan(0, -4, 0, 0, 0)), new Guid("98741b77-36dc-4df3-b47f-a5ef3e709925"), "CompanyAdmin", new DateTimeOffset(new DateTime(2024, 4, 24, 12, 48, 56, 886, DateTimeKind.Unspecified).AddTicks(7151), new TimeSpan(0, -4, 0, 0, 0)), "C2VVWCY9WX" },
                    { new Guid("50696915-9dca-4753-9c1c-3317d9c3062c"), "system", new DateTimeOffset(new DateTime(2024, 4, 24, 12, 48, 56, 886, DateTimeKind.Unspecified).AddTicks(7079), new TimeSpan(0, -4, 0, 0, 0)), new Guid("2e3887da-2ff7-4554-b73f-7eb1d95d2bf1"), "CompanyAdmin", new DateTimeOffset(new DateTime(2024, 4, 24, 12, 48, 56, 886, DateTimeKind.Unspecified).AddTicks(7131), new TimeSpan(0, -4, 0, 0, 0)), "2XHKG7BFX7" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TenantInfo_Identifier",
                table: "TenantInfo",
                column: "Identifier",
                unique: true,
                filter: "[Identifier] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TenantInfo");

            migrationBuilder.DeleteData(
                table: "UserInvitations",
                keyColumn: "Id",
                keyValue: new Guid("2c5dda4c-78a6-4f7f-a991-964a7f199943"));

            migrationBuilder.DeleteData(
                table: "UserInvitations",
                keyColumn: "Id",
                keyValue: new Guid("50696915-9dca-4753-9c1c-3317d9c3062c"));

            migrationBuilder.InsertData(
                table: "UserInvitations",
                columns: new[] { "Id", "CreatedBy", "CreatedTime", "CustomerId", "DelegatedUserManagementRole", "ExpiresTime", "InvitationCode" },
                values: new object[,]
                {
                    { new Guid("24ba9736-b7ae-411c-a516-f1f8cff0d251"), "system", new DateTimeOffset(new DateTime(2024, 4, 24, 10, 43, 10, 314, DateTimeKind.Unspecified).AddTicks(8987), new TimeSpan(0, -4, 0, 0, 0)), new Guid("68f731f3-5cda-4b3d-97e6-72439842d522"), "CompanyAdmin", new DateTimeOffset(new DateTime(2024, 4, 24, 10, 43, 10, 314, DateTimeKind.Unspecified).AddTicks(9034), new TimeSpan(0, -4, 0, 0, 0)), "HP4JXHMTCJ" },
                    { new Guid("9911db18-e8ba-4b83-8c4f-2676e8fc8cfb"), "system", new DateTimeOffset(new DateTime(2024, 4, 24, 10, 43, 10, 314, DateTimeKind.Unspecified).AddTicks(9043), new TimeSpan(0, -4, 0, 0, 0)), new Guid("83ceec59-d9a5-4031-8347-fc4a1d516147"), "CompanyAdmin", new DateTimeOffset(new DateTime(2024, 4, 24, 10, 43, 10, 314, DateTimeKind.Unspecified).AddTicks(9045), new TimeSpan(0, -4, 0, 0, 0)), "QJHM87VD49" }
                });
        }
    }
}
