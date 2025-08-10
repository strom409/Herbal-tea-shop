using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcHer.Migrations
{
    /// <inheritdoc />
    public partial class AddBannerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Subtitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ButtonText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ButtonUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 9, 13, 45, 28, 607, DateTimeKind.Utc).AddTicks(2498), new DateTime(2025, 8, 9, 13, 45, 28, 607, DateTimeKind.Utc).AddTicks(2500) });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 9, 13, 45, 28, 607, DateTimeKind.Utc).AddTicks(2507), new DateTime(2025, 8, 9, 13, 45, 28, 607, DateTimeKind.Utc).AddTicks(2508) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 9, 13, 45, 28, 607, DateTimeKind.Utc).AddTicks(2389), new DateTime(2025, 8, 9, 13, 45, 28, 607, DateTimeKind.Utc).AddTicks(2390) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 9, 13, 45, 28, 607, DateTimeKind.Utc).AddTicks(2397), new DateTime(2025, 8, 9, 13, 45, 28, 607, DateTimeKind.Utc).AddTicks(2398) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 9, 13, 45, 28, 607, DateTimeKind.Utc).AddTicks(2402), new DateTime(2025, 8, 9, 13, 45, 28, 607, DateTimeKind.Utc).AddTicks(2403) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 9, 13, 45, 28, 607, DateTimeKind.Utc).AddTicks(2407), new DateTime(2025, 8, 9, 13, 45, 28, 607, DateTimeKind.Utc).AddTicks(2408) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 9, 13, 45, 28, 607, DateTimeKind.Utc).AddTicks(2412), new DateTime(2025, 8, 9, 13, 45, 28, 607, DateTimeKind.Utc).AddTicks(2413) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 9, 13, 45, 28, 607, DateTimeKind.Utc).AddTicks(2418), new DateTime(2025, 8, 9, 13, 45, 28, 607, DateTimeKind.Utc).AddTicks(2419) });

            migrationBuilder.UpdateData(
                table: "SocialLinks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 9, 13, 45, 28, 607, DateTimeKind.Utc).AddTicks(2871), new DateTime(2025, 8, 9, 13, 45, 28, 607, DateTimeKind.Utc).AddTicks(2872) });

            migrationBuilder.UpdateData(
                table: "SocialLinks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 9, 13, 45, 28, 607, DateTimeKind.Utc).AddTicks(2987), new DateTime(2025, 8, 9, 13, 45, 28, 607, DateTimeKind.Utc).AddTicks(2989) });

            migrationBuilder.UpdateData(
                table: "SocialLinks",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 9, 13, 45, 28, 607, DateTimeKind.Utc).AddTicks(2998), new DateTime(2025, 8, 9, 13, 45, 28, 607, DateTimeKind.Utc).AddTicks(2999) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8292), new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8293) });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8299), new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8299) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8203), new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8204) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8209), new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8210) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8214), new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8215) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8218), new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8219) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8223), new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8223) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8227), new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8228) });

            migrationBuilder.UpdateData(
                table: "SocialLinks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8492), new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8493) });

            migrationBuilder.UpdateData(
                table: "SocialLinks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8498), new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8499) });

            migrationBuilder.UpdateData(
                table: "SocialLinks",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8502), new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8503) });
        }
    }
}
