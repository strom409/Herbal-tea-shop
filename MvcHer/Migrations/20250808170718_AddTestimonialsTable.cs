using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MvcHer.Migrations
{
    /// <inheritdoc />
    public partial class AddTestimonialsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Testimonials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClientProfession = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TestimonialText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ClientImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    ClientCompany = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ClientLocation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AdminNotes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testimonials", x => x.Id);
                });

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

            migrationBuilder.InsertData(
                table: "Testimonials",
                columns: new[] { "Id", "AdminNotes", "ClientCompany", "ClientImageUrl", "ClientLocation", "ClientName", "ClientProfession", "CreatedAt", "IsActive", "IsApproved", "IsFeatured", "Rating", "TestimonialText", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, null, "Johnson & Associates", "~/img/testimonial-1.jpg", "New York, USA", "Sarah Johnson", "Tea Enthusiast", new DateTime(2024, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc), true, true, true, 5, "The quality of tea from Herbal Tea is absolutely exceptional! Every cup is a journey of flavors that awakens my senses. Their Earl Grey has become my daily ritual, and I can't imagine starting my morning without it.", new DateTime(2024, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, "Golden Dragon Restaurant", "~/img/testimonial-2.jpg", "San Francisco, USA", "Michael Chen", "Restaurant Owner", new DateTime(2024, 11, 2, 0, 0, 0, 0, DateTimeKind.Utc), true, true, true, 5, "As a restaurant owner, I serve Herbal Tea's premium blends to my customers, and they absolutely love them! The consistency in quality and the rich, authentic flavors have made our tea service a highlight of the dining experience.", new DateTime(2024, 11, 2, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, null, "Wellness First Coaching", "~/img/testimonial-3.jpg", "Los Angeles, USA", "Emma Williams", "Wellness Coach", new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), true, true, true, 5, "I recommend Herbal Tea to all my clients seeking natural wellness solutions. Their organic herbal blends are pure, potent, and perfectly crafted. The chamomile tea has helped many of my clients achieve better sleep quality.", new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Testimonials",
                columns: new[] { "Id", "AdminNotes", "ClientCompany", "ClientImageUrl", "ClientLocation", "ClientName", "ClientProfession", "CreatedAt", "IsActive", "IsApproved", "Rating", "TestimonialText", "UpdatedAt" },
                values: new object[,]
                {
                    { 4, null, "TechCorp Industries", "~/img/testimonial-4.jpg", "Seattle, USA", "David Kumar", "Corporate Executive", new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), true, true, 5, "Working long hours in the corporate world, I need something that helps me stay focused and energized. Herbal Tea's green tea collection has become my secret weapon for maintaining productivity while enjoying moments of calm.", new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, null, "Zen Yoga Studio", "~/img/testimonial-5.jpg", "Portland, USA", "Lisa Thompson", "Yoga Instructor", new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), true, true, 5, "The mindful experience of drinking Herbal Tea perfectly complements my yoga practice. Each sip brings tranquility and balance. I especially love their jasmine green tea - it's like meditation in a cup!", new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Testimonials");

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
    }
}
