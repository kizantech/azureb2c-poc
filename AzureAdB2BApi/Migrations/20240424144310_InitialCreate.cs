using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AzureAdB2BApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserInvitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvitationCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExpiresTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DelegatedUserManagementRole = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInvitations", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "UserInvitations",
                columns: new[] { "Id", "CreatedBy", "CreatedTime", "CustomerId", "DelegatedUserManagementRole", "ExpiresTime", "InvitationCode" },
                values: new object[,]
                {
                    { new Guid("24ba9736-b7ae-411c-a516-f1f8cff0d251"), "system", new DateTimeOffset(new DateTime(2024, 4, 24, 10, 43, 10, 314, DateTimeKind.Unspecified).AddTicks(8987), new TimeSpan(0, -4, 0, 0, 0)), new Guid("68f731f3-5cda-4b3d-97e6-72439842d522"), "CompanyAdmin", new DateTimeOffset(new DateTime(2024, 4, 24, 10, 43, 10, 314, DateTimeKind.Unspecified).AddTicks(9034), new TimeSpan(0, -4, 0, 0, 0)), "HP4JXHMTCJ" },
                    { new Guid("9911db18-e8ba-4b83-8c4f-2676e8fc8cfb"), "system", new DateTimeOffset(new DateTime(2024, 4, 24, 10, 43, 10, 314, DateTimeKind.Unspecified).AddTicks(9043), new TimeSpan(0, -4, 0, 0, 0)), new Guid("83ceec59-d9a5-4031-8347-fc4a1d516147"), "CompanyAdmin", new DateTimeOffset(new DateTime(2024, 4, 24, 10, 43, 10, 314, DateTimeKind.Unspecified).AddTicks(9045), new TimeSpan(0, -4, 0, 0, 0)), "QJHM87VD49" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInvitations");
        }
    }
}
