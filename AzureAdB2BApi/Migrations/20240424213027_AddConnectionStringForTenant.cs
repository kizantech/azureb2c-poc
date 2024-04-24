using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AzureAdB2BApi.Migrations
{
    /// <inheritdoc />
    public partial class AddConnectionStringForTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserInvitations",
                keyColumn: "Id",
                keyValue: new Guid("2c5dda4c-78a6-4f7f-a991-964a7f199943"));

            migrationBuilder.DeleteData(
                table: "UserInvitations",
                keyColumn: "Id",
                keyValue: new Guid("50696915-9dca-4753-9c1c-3317d9c3062c"));

            migrationBuilder.AddColumn<string>(
                name: "ConnectionString",
                table: "TenantInfo",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionString",
                table: "TenantInfo");

            migrationBuilder.InsertData(
                table: "UserInvitations",
                columns: new[] { "Id", "CreatedBy", "CreatedTime", "CustomerId", "DelegatedUserManagementRole", "ExpiresTime", "InvitationCode" },
                values: new object[,]
                {
                    { new Guid("2c5dda4c-78a6-4f7f-a991-964a7f199943"), "system", new DateTimeOffset(new DateTime(2024, 4, 24, 12, 48, 56, 886, DateTimeKind.Unspecified).AddTicks(7150), new TimeSpan(0, -4, 0, 0, 0)), new Guid("98741b77-36dc-4df3-b47f-a5ef3e709925"), "CompanyAdmin", new DateTimeOffset(new DateTime(2024, 4, 24, 12, 48, 56, 886, DateTimeKind.Unspecified).AddTicks(7151), new TimeSpan(0, -4, 0, 0, 0)), "C2VVWCY9WX" },
                    { new Guid("50696915-9dca-4753-9c1c-3317d9c3062c"), "system", new DateTimeOffset(new DateTime(2024, 4, 24, 12, 48, 56, 886, DateTimeKind.Unspecified).AddTicks(7079), new TimeSpan(0, -4, 0, 0, 0)), new Guid("2e3887da-2ff7-4554-b73f-7eb1d95d2bf1"), "CompanyAdmin", new DateTimeOffset(new DateTime(2024, 4, 24, 12, 48, 56, 886, DateTimeKind.Unspecified).AddTicks(7131), new TimeSpan(0, -4, 0, 0, 0)), "2XHKG7BFX7" }
                });
        }
    }
}
