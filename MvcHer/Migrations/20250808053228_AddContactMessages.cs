using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcHer.Migrations
{
    /// <inheritdoc />
    public partial class AddContactMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AdminResponse = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ResponseDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMessages", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "$2a$11$K8C1Lhqz7Q2d.5wjwMqBdOXnDlHkVGWyBjKr5o8FqU2YvMqBdOXnDl", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 5, 32, 25, 248, DateTimeKind.Utc).AddTicks(48), new DateTime(2025, 8, 8, 5, 32, 25, 248, DateTimeKind.Utc).AddTicks(50) });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 5, 32, 25, 248, DateTimeKind.Utc).AddTicks(57), new DateTime(2025, 8, 8, 5, 32, 25, 248, DateTimeKind.Utc).AddTicks(58) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 5, 32, 25, 247, DateTimeKind.Utc).AddTicks(9913), new DateTime(2025, 8, 8, 5, 32, 25, 247, DateTimeKind.Utc).AddTicks(9914) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 5, 32, 25, 247, DateTimeKind.Utc).AddTicks(9922), new DateTime(2025, 8, 8, 5, 32, 25, 247, DateTimeKind.Utc).AddTicks(9923) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 5, 32, 25, 247, DateTimeKind.Utc).AddTicks(9929), new DateTime(2025, 8, 8, 5, 32, 25, 247, DateTimeKind.Utc).AddTicks(9930) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 5, 32, 25, 247, DateTimeKind.Utc).AddTicks(9935), new DateTime(2025, 8, 8, 5, 32, 25, 247, DateTimeKind.Utc).AddTicks(9936) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 5, 32, 25, 247, DateTimeKind.Utc).AddTicks(9941), new DateTime(2025, 8, 8, 5, 32, 25, 247, DateTimeKind.Utc).AddTicks(9942) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 5, 32, 25, 247, DateTimeKind.Utc).AddTicks(9947), new DateTime(2025, 8, 8, 5, 32, 25, 247, DateTimeKind.Utc).AddTicks(9948) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactMessages");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 7, 17, 25, 49, 228, DateTimeKind.Utc).AddTicks(3426), "$2a$11$xLn7cN78BrCJluJAyjot7eT8mdJOgPsKZEFaSI18DhK2dKMwDs7zG", new DateTime(2025, 8, 7, 17, 25, 49, 228, DateTimeKind.Utc).AddTicks(3429) });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 7, 17, 25, 49, 228, DateTimeKind.Utc).AddTicks(4538), new DateTime(2025, 8, 7, 17, 25, 49, 228, DateTimeKind.Utc).AddTicks(4554) });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 7, 17, 25, 49, 228, DateTimeKind.Utc).AddTicks(4561), new DateTime(2025, 8, 7, 17, 25, 49, 228, DateTimeKind.Utc).AddTicks(4561) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 7, 17, 25, 49, 228, DateTimeKind.Utc).AddTicks(4362), new DateTime(2025, 8, 7, 17, 25, 49, 228, DateTimeKind.Utc).AddTicks(4363) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 7, 17, 25, 49, 228, DateTimeKind.Utc).AddTicks(4374), new DateTime(2025, 8, 7, 17, 25, 49, 228, DateTimeKind.Utc).AddTicks(4374) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 7, 17, 25, 49, 228, DateTimeKind.Utc).AddTicks(4380), new DateTime(2025, 8, 7, 17, 25, 49, 228, DateTimeKind.Utc).AddTicks(4382) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 7, 17, 25, 49, 228, DateTimeKind.Utc).AddTicks(4388), new DateTime(2025, 8, 7, 17, 25, 49, 228, DateTimeKind.Utc).AddTicks(4389) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 7, 17, 25, 49, 228, DateTimeKind.Utc).AddTicks(4398), new DateTime(2025, 8, 7, 17, 25, 49, 228, DateTimeKind.Utc).AddTicks(4398) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 7, 17, 25, 49, 228, DateTimeKind.Utc).AddTicks(4402), new DateTime(2025, 8, 7, 17, 25, 49, 228, DateTimeKind.Utc).AddTicks(4404) });
        }
    }
}
