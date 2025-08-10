using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MvcHer.Migrations
{
    /// <inheritdoc />
    public partial class AddSocialLinksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SocialLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Platform = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IconClass = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialLinks", x => x.Id);
                });

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

            migrationBuilder.InsertData(
                table: "SocialLinks",
                columns: new[] { "Id", "Color", "CreatedAt", "DisplayName", "DisplayOrder", "IconClass", "IsActive", "Platform", "UpdatedAt", "Url" },
                values: new object[,]
                {
                    { 1, "#1877F2", new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8492), "Herbal Tea Facebook", 1, "fab fa-facebook-f", true, "Facebook", new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8493), "https://facebook.com/herbaltea" },
                    { 2, "#1DA1F2", new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8498), "Herbal Tea Twitter", 2, "fab fa-twitter", true, "Twitter", new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8499), "https://twitter.com/herbaltea" },
                    { 3, "#E4405F", new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8502), "Herbal Tea Instagram", 3, "fab fa-instagram", true, "Instagram", new DateTime(2025, 8, 8, 17, 54, 0, 609, DateTimeKind.Utc).AddTicks(8503), "https://instagram.com/herbaltea" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocialLinks");

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
    }
}
