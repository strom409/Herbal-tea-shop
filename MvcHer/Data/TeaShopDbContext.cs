using Microsoft.EntityFrameworkCore;
using MvcHer.Models;

namespace MvcHer.Data
{
    public class TeaShopDbContext : DbContext
    {
        public TeaShopDbContext(DbContextOptions<TeaShopDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<SocialLink> SocialLinks { get; set; }
        public DbSet<Banner> Banners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.SKU).IsUnique();
                entity.Property(e => e.Price).HasPrecision(18, 2);
            });

            // Configure Customer
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.OrderNumber).IsUnique();
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                
                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.Orders)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure OrderItem
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
                
                entity.HasOne(e => e.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure CartItem
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.CartItems)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Ensure unique cart items per customer-product combination
                entity.HasIndex(e => new { e.CustomerId, e.ProductId }).IsUnique();
            });

            // Configure Admin
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure AboutUs
            modelBuilder.Entity<AboutUs>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Subtitle).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.FounderName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FounderTitle).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FounderMessage).IsRequired();
                entity.Property(e => e.Mission).IsRequired();
                entity.Property(e => e.Vision).IsRequired();
                entity.Property(e => e.Values).IsRequired();
                entity.Property(e => e.ImageUrl).HasMaxLength(255);
                entity.Property(e => e.FounderImageUrl).HasMaxLength(255);
                entity.Property(e => e.Awards).HasMaxLength(500);
                entity.Property(e => e.Certifications).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            // Configure Testimonial
            modelBuilder.Entity<Testimonial>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ClientName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ClientProfession).IsRequired().HasMaxLength(100);
                entity.Property(e => e.TestimonialText).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.ClientImageUrl).HasMaxLength(255);
                entity.Property(e => e.ClientCompany).HasMaxLength(100);
                entity.Property(e => e.ClientLocation).HasMaxLength(50);
                entity.Property(e => e.AdminNotes).HasMaxLength(200);
                entity.Property(e => e.Rating).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.IsApproved).HasDefaultValue(true);
                entity.Property(e => e.IsFeatured).HasDefaultValue(false);
            });

            // Configure SocialLink
            modelBuilder.Entity<SocialLink>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Platform).IsRequired().HasMaxLength(50);
                entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Url).IsRequired().HasMaxLength(500);
                entity.Property(e => e.IconClass).HasMaxLength(50);
                entity.Property(e => e.Color).HasMaxLength(20);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
            });

            // Configure Banner
            modelBuilder.Entity<Banner>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Subtitle).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(500);
                entity.Property(e => e.ButtonText).HasMaxLength(200);
                entity.Property(e => e.ButtonUrl).HasMaxLength(500);
                entity.Property(e => e.DisplayOrder).HasDefaultValue(1);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Admin
            // Pre-generated BCrypt hash for "admin123" - $2a$11$K8C1Lhqz7Q2d.5wjwMqBdOXnDlHkVGWyBjKr5o8FqU2YvMqBdOXnDl
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = "$2a$11$K8C1Lhqz7Q2d.5wjwMqBdOXnDlHkVGWyBjKr5o8FqU2YvMqBdOXnDl",
                    FirstName = "System",
                    LastName = "Administrator",
                    Email = "admin@herbal-tea.com",
                    Role = "SuperAdmin",
                    IsActive = true,
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Premium Green Tea",
                    SKU = "GT001",
                    Description = "Premium quality green tea with natural antioxidants and health benefits. Sourced from the finest tea gardens.",
                    Price = 15.00m,
                    StockQuantity = 45,
                    Category = "Green Tea",
                    ImageUrl = "~/img/product-1.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 2,
                    Name = "Earl Grey Black Tea",
                    SKU = "BT001",
                    Description = "Rich and robust black tea with full-bodied flavor and aromatic bergamot oil.",
                    Price = 18.00m,
                    StockQuantity = 32,
                    Category = "Black Tea",
                    ImageUrl = "~/img/product-2.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 3,
                    Name = "Chai Spiced Tea",
                    SKU = "ST001",
                    Description = "Aromatic blend of traditional spices and tea for a warming and invigorating experience.",
                    Price = 20.00m,
                    StockQuantity = 28,
                    Category = "Spiced Tea",
                    ImageUrl = "~/img/product-3.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 4,
                    Name = "Organic Herbal Tea",
                    SKU = "OT001",
                    Description = "Certified organic herbal tea grown without pesticides or chemicals. Pure and natural.",
                    Price = 22.00m,
                    StockQuantity = 15,
                    Category = "Organic Tea",
                    ImageUrl = "~/img/product-4.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 5,
                    Name = "Chamomile Herbal Tea",
                    SKU = "HT001",
                    Description = "Relaxing chamomile tea perfect for evening consumption and better sleep quality.",
                    Price = 16.00m,
                    StockQuantity = 38,
                    Category = "Herbal Tea",
                    ImageUrl = "~/img/store-product-2.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 6,
                    Name = "Jasmine Green Tea",
                    SKU = "GT002",
                    Description = "Fragrant jasmine green tea with delicate floral notes and refreshing taste.",
                    Price = 24.00m,
                    StockQuantity = 22,
                    Category = "Green Tea",
                    ImageUrl = "~/img/store-product-3.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );

            // Seed Sample Customer
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "john.smith@email.com",
                    PhoneNumber = "+1-555-0123",
                    Address = "123 Main Street",
                    City = "New York",
                    State = "NY",
                    ZipCode = "10001",
                    Country = "USA",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Customer
                {
                    Id = 2,
                    FirstName = "Sarah",
                    LastName = "Johnson",
                    Email = "sarah.johnson@email.com",
                    PhoneNumber = "+1-555-0124",
                    Address = "456 Oak Avenue",
                    City = "Los Angeles",
                    State = "CA",
                    ZipCode = "90210",
                    Country = "USA",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );

            // Seed AboutUs
            modelBuilder.Entity<AboutUs>().HasData(
                new AboutUs
                {
                    Id = 1,
                    Title = "Welcome to Herbal Tea - Your Premium Tea Destination",
                    Subtitle = "Crafting exceptional tea experiences since 2010 with passion, quality, and tradition",
                    Description = "At Herbal Tea, we believe that every cup tells a story. Our journey began with a simple mission: to bring the finest, most authentic tea experiences directly to your doorstep. We source our premium teas from the world's most renowned tea gardens, ensuring that each blend captures the essence of its origin while delivering unparalleled quality and flavor.",
                    ImageUrl = "~/img/about-hero.jpg",
                    FounderName = "Rajesh Kumar",
                    FounderTitle = "Founder & Master Tea Curator",
                    FounderMessage = "My passion for tea began during my travels across the tea gardens of Assam and Darjeeling. I witnessed firsthand the dedication of tea artisans who have perfected their craft over generations. This inspired me to create Herbal Tea - a bridge between these master craftsmen and tea lovers worldwide. Every blend we offer is a testament to this commitment to excellence.",
                    FounderImageUrl = "~/img/founder.jpg",
                    Mission = "To deliver the world's finest teas while supporting sustainable farming practices and empowering tea communities. We are committed to bringing authentic, premium tea experiences that connect people with the rich heritage and culture of tea.",
                    Vision = "To become the global leader in premium tea retail, recognized for our commitment to quality, sustainability, and customer satisfaction. We envision a world where every tea lover has access to exceptional teas that inspire moments of peace and connection.",
                    Values = "Quality First: We never compromise on the quality of our teas. Sustainability: We support eco-friendly farming practices. Authenticity: Every tea tells its authentic story. Customer Focus: Your satisfaction is our priority. Community: We believe in giving back to tea-growing communities.",
                    YearsOfExperience = 14,
                    HappyCustomers = 25000,
                    TeaVarieties = 150,
                    CountriesServed = 35,
                    Awards = "Best Tea Retailer 2023, Organic Certification Excellence Award 2022, Customer Choice Award 2021",
                    Certifications = "Organic Certified, Fair Trade Certified, ISO 22000 Food Safety, HACCP Certified",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true
                }
            );

            // Seed Testimonials
            modelBuilder.Entity<Testimonial>().HasData(
                new Testimonial
                {
                    Id = 1,
                    ClientName = "Sarah Johnson",
                    ClientProfession = "Tea Enthusiast",
                    TestimonialText = "The quality of tea from Herbal Tea is absolutely exceptional! Every cup is a journey of flavors that awakens my senses. Their Earl Grey has become my daily ritual, and I can't imagine starting my morning without it.",
                    ClientImageUrl = "~/img/testimonial-1.jpg",
                    Rating = 5,
                    ClientCompany = "Johnson & Associates",
                    ClientLocation = "New York, USA",
                    IsFeatured = true,
                    CreatedAt = new DateTime(2024, 10, 15, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 10, 15, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true,
                    IsApproved = true
                },
                new Testimonial
                {
                    Id = 2,
                    ClientName = "Michael Chen",
                    ClientProfession = "Restaurant Owner",
                    TestimonialText = "As a restaurant owner, I serve Herbal Tea's premium blends to my customers, and they absolutely love them! The consistency in quality and the rich, authentic flavors have made our tea service a highlight of the dining experience.",
                    ClientImageUrl = "~/img/testimonial-2.jpg",
                    Rating = 5,
                    ClientCompany = "Golden Dragon Restaurant",
                    ClientLocation = "San Francisco, USA",
                    IsFeatured = true,
                    CreatedAt = new DateTime(2024, 11, 2, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 2, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true,
                    IsApproved = true
                },
                new Testimonial
                {
                    Id = 3,
                    ClientName = "Emma Williams",
                    ClientProfession = "Wellness Coach",
                    TestimonialText = "I recommend Herbal Tea to all my clients seeking natural wellness solutions. Their organic herbal blends are pure, potent, and perfectly crafted. The chamomile tea has helped many of my clients achieve better sleep quality.",
                    ClientImageUrl = "~/img/testimonial-3.jpg",
                    Rating = 5,
                    ClientCompany = "Wellness First Coaching",
                    ClientLocation = "Los Angeles, USA",
                    IsFeatured = true,
                    CreatedAt = new DateTime(2024, 11, 20, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 11, 20, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true,
                    IsApproved = true
                },
                new Testimonial
                {
                    Id = 4,
                    ClientName = "David Kumar",
                    ClientProfession = "Corporate Executive",
                    TestimonialText = "Working long hours in the corporate world, I need something that helps me stay focused and energized. Herbal Tea's green tea collection has become my secret weapon for maintaining productivity while enjoying moments of calm.",
                    ClientImageUrl = "~/img/testimonial-4.jpg",
                    Rating = 5,
                    ClientCompany = "TechCorp Industries",
                    ClientLocation = "Seattle, USA",
                    IsFeatured = false,
                    CreatedAt = new DateTime(2024, 12, 5, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 12, 5, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true,
                    IsApproved = true
                },
                new Testimonial
                {
                    Id = 5,
                    ClientName = "Lisa Thompson",
                    ClientProfession = "Yoga Instructor",
                    TestimonialText = "The mindful experience of drinking Herbal Tea perfectly complements my yoga practice. Each sip brings tranquility and balance. I especially love their jasmine green tea - it's like meditation in a cup!",
                    ClientImageUrl = "~/img/testimonial-5.jpg",
                    Rating = 5,
                    ClientCompany = "Zen Yoga Studio",
                    ClientLocation = "Portland, USA",
                    IsFeatured = false,
                    CreatedAt = new DateTime(2024, 12, 18, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 12, 18, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true,
                    IsApproved = true
                }
            );

            // Seed SocialLinks
            modelBuilder.Entity<SocialLink>().HasData(
                new SocialLink
                {
                    Id = 1,
                    Platform = "Facebook",
                    DisplayName = "Herbal Tea Facebook",
                    Url = "https://facebook.com/herbaltea",
                    IconClass = "fab fa-facebook-f",
                    Color = "#1877F2",
                    IsActive = true,
                    DisplayOrder = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new SocialLink
                {
                    Id = 2,
                    Platform = "Twitter",
                    DisplayName = "Herbal Tea Twitter",
                    Url = "https://twitter.com/herbaltea",
                    IconClass = "fab fa-twitter",
                    Color = "#1DA1F2",
                    IsActive = true,
                    DisplayOrder = 2,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new SocialLink
                {
                    Id = 3,
                    Platform = "Instagram",
                    DisplayName = "Herbal Tea Instagram",
                    Url = "https://instagram.com/herbaltea",
                    IconClass = "fab fa-instagram",
                    Color = "#E4405F",
                    IsActive = true,
                    DisplayOrder = 3,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
            
            // Configure ContactMessage
            modelBuilder.Entity<ContactMessage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Subject).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.AdminResponse).HasMaxLength(1000);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.IsRead).HasDefaultValue(false);
            });
        }
    }
}
