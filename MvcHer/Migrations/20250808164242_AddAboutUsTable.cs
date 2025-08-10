using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcHer.Migrations
{
    /// <inheritdoc />
    public partial class AddAboutUsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutUs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Subtitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FounderName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FounderTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FounderMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FounderImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Mission = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vision = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Values = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: false),
                    HappyCustomers = table.Column<int>(type: "int", nullable: false),
                    TeaVarieties = table.Column<int>(type: "int", nullable: false),
                    CountriesServed = table.Column<int>(type: "int", nullable: false),
                    Awards = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Certifications = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AboutUs",
                columns: new[] { "Id", "Awards", "Certifications", "CountriesServed", "CreatedAt", "Description", "FounderImageUrl", "FounderMessage", "FounderName", "FounderTitle", "HappyCustomers", "ImageUrl", "IsActive", "Mission", "Subtitle", "TeaVarieties", "Title", "UpdatedAt", "Values", "Vision", "YearsOfExperience" },
                values: new object[] { 1, "Best Tea Retailer 2023, Organic Certification Excellence Award 2022, Customer Choice Award 2021", "Organic Certified, Fair Trade Certified, ISO 22000 Food Safety, HACCP Certified", 35, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "At Herbal Tea, we believe that every cup tells a story. Our journey began with a simple mission: to bring the finest, most authentic tea experiences directly to your doorstep. We source our premium teas from the world's most renowned tea gardens, ensuring that each blend captures the essence of its origin while delivering unparalleled quality and flavor.", "~/img/founder.jpg", "My passion for tea began during my travels across the tea gardens of Assam and Darjeeling. I witnessed firsthand the dedication of tea artisans who have perfected their craft over generations. This inspired me to create Herbal Tea - a bridge between these master craftsmen and tea lovers worldwide. Every blend we offer is a testament to this commitment to excellence.", "Rajesh Kumar", "Founder & Master Tea Curator", 25000, "~/img/about-hero.jpg", true, "To deliver the world's finest teas while supporting sustainable farming practices and empowering tea communities. We are committed to bringing authentic, premium tea experiences that connect people with the rich heritage and culture of tea.", "Crafting exceptional tea experiences since 2010 with passion, quality, and tradition", 150, "Welcome to Herbal Tea - Your Premium Tea Destination", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Quality First: We never compromise on the quality of our teas. Sustainability: We support eco-friendly farming practices. Authenticity: Every tea tells its authentic story. Customer Focus: Your satisfaction is our priority. Community: We believe in giving back to tea-growing communities.", "To become the global leader in premium tea retail, recognized for our commitment to quality, sustainability, and customer satisfaction. We envision a world where every tea lover has access to exceptional teas that inspire moments of peace and connection.", 14 });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 16, 42, 41, 756, DateTimeKind.Utc).AddTicks(4686), new DateTime(2025, 8, 8, 16, 42, 41, 756, DateTimeKind.Utc).AddTicks(4687) });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 16, 42, 41, 756, DateTimeKind.Utc).AddTicks(4693), new DateTime(2025, 8, 8, 16, 42, 41, 756, DateTimeKind.Utc).AddTicks(4694) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 16, 42, 41, 756, DateTimeKind.Utc).AddTicks(4582), new DateTime(2025, 8, 8, 16, 42, 41, 756, DateTimeKind.Utc).AddTicks(4583) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 16, 42, 41, 756, DateTimeKind.Utc).AddTicks(4589), new DateTime(2025, 8, 8, 16, 42, 41, 756, DateTimeKind.Utc).AddTicks(4590) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 16, 42, 41, 756, DateTimeKind.Utc).AddTicks(4594), new DateTime(2025, 8, 8, 16, 42, 41, 756, DateTimeKind.Utc).AddTicks(4594) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 16, 42, 41, 756, DateTimeKind.Utc).AddTicks(4598), new DateTime(2025, 8, 8, 16, 42, 41, 756, DateTimeKind.Utc).AddTicks(4598) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 16, 42, 41, 756, DateTimeKind.Utc).AddTicks(4603), new DateTime(2025, 8, 8, 16, 42, 41, 756, DateTimeKind.Utc).AddTicks(4604) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 8, 16, 42, 41, 756, DateTimeKind.Utc).AddTicks(4607), new DateTime(2025, 8, 8, 16, 42, 41, 756, DateTimeKind.Utc).AddTicks(4608) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutUs");

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
    }
}
