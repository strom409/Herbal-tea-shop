using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcHer.Migrations
{
    /// <inheritdoc />
    public partial class AddPhoneAndSecondaryContactFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "ContactMessages",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryEmail",
                table: "ContactMessages",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryPhone",
                table: "ContactMessages",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 22, 53, 712, DateTimeKind.Utc).AddTicks(1192), new DateTime(2025, 8, 8, 17, 22, 53, 712, DateTimeKind.Utc).AddTicks(1193) });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 22, 53, 712, DateTimeKind.Utc).AddTicks(1197), new DateTime(2025, 8, 8, 17, 22, 53, 712, DateTimeKind.Utc).AddTicks(1198) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 22, 53, 712, DateTimeKind.Utc).AddTicks(1133), new DateTime(2025, 8, 8, 17, 22, 53, 712, DateTimeKind.Utc).AddTicks(1133) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 22, 53, 712, DateTimeKind.Utc).AddTicks(1138), new DateTime(2025, 8, 8, 17, 22, 53, 712, DateTimeKind.Utc).AddTicks(1139) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 22, 53, 712, DateTimeKind.Utc).AddTicks(1142), new DateTime(2025, 8, 8, 17, 22, 53, 712, DateTimeKind.Utc).AddTicks(1142) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 22, 53, 712, DateTimeKind.Utc).AddTicks(1145), new DateTime(2025, 8, 8, 17, 22, 53, 712, DateTimeKind.Utc).AddTicks(1146) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 22, 53, 712, DateTimeKind.Utc).AddTicks(1149), new DateTime(2025, 8, 8, 17, 22, 53, 712, DateTimeKind.Utc).AddTicks(1149) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 22, 53, 712, DateTimeKind.Utc).AddTicks(1152), new DateTime(2025, 8, 8, 17, 22, 53, 712, DateTimeKind.Utc).AddTicks(1153) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "SecondaryEmail",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "SecondaryPhone",
                table: "ContactMessages");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 7, 17, 490, DateTimeKind.Utc).AddTicks(5950), new DateTime(2025, 8, 8, 17, 7, 17, 490, DateTimeKind.Utc).AddTicks(5951) });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 7, 17, 490, DateTimeKind.Utc).AddTicks(5965), new DateTime(2025, 8, 8, 17, 7, 17, 490, DateTimeKind.Utc).AddTicks(5967) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 7, 17, 490, DateTimeKind.Utc).AddTicks(5698), new DateTime(2025, 8, 8, 17, 7, 17, 490, DateTimeKind.Utc).AddTicks(5700) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 7, 17, 490, DateTimeKind.Utc).AddTicks(5714), new DateTime(2025, 8, 8, 17, 7, 17, 490, DateTimeKind.Utc).AddTicks(5716) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 7, 17, 490, DateTimeKind.Utc).AddTicks(5727), new DateTime(2025, 8, 8, 17, 7, 17, 490, DateTimeKind.Utc).AddTicks(5729) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 7, 17, 490, DateTimeKind.Utc).AddTicks(5740), new DateTime(2025, 8, 8, 17, 7, 17, 490, DateTimeKind.Utc).AddTicks(5742) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 7, 17, 490, DateTimeKind.Utc).AddTicks(5752), new DateTime(2025, 8, 8, 17, 7, 17, 490, DateTimeKind.Utc).AddTicks(5754) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 7, 17, 490, DateTimeKind.Utc).AddTicks(5765), new DateTime(2025, 8, 8, 17, 7, 17, 490, DateTimeKind.Utc).AddTicks(5766) });
        }
    }
}
