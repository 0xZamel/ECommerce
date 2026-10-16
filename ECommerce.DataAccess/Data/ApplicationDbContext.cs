using ECommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using System.Xml;

namespace ECommerce.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet <Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);



            builder.Entity<AuditLog>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Wishlist>()
                .HasOne(w => w.User)
                .WithMany()
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Wishlist>()
                .HasOne(w => w.Product)
                .WithMany()
                .HasForeignKey(w => w.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Wishlist>()
                .HasIndex(w => new { w.UserId, w.ProductId })
                .IsUnique();

            builder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            var adminUser = new ApplicationUser
            {
                Id = "admin-user-1",
                UserName = "admin@ecommerce.com",
                NormalizedUserName = "ADMIN@ECOMMERCE.COM",
                Email = "admin@ecommerce.com",
                NormalizedEmail = "ADMIN@ECOMMERCE.COM",
                EmailConfirmed = true,
                FullName = "Administrator",
                SecurityStamp = "admin-security-stamp",
                ConcurrencyStamp = "admin-concurrency-stamp",
                PasswordHash = "AQAAAAIAAYagAAAAELBMmljqmxAOv6FsyPqD+fdAMgA+OrooLFkRHSB5oH0rWhtg8lXgqOrp9izM9810pw=="
            };



            builder.Entity<ApplicationUser>().HasData(adminUser);

            builder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .IsRequired();
            builder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Electronics",
                    DisplayOrder = 1
                },
                new Category
                {
                    Id = 2,
                    Name = "Computers",
                    DisplayOrder = 2
                },
                new Category
                {
                    Id = 3,
                    Name = "Accessories",
                    DisplayOrder = 3
                },
                new Category
                {
                    Id = 4,
                    Name = "Gaming",
                    DisplayOrder = 4
                },
                new Category
                {
                    Id = 5,
                    Name = "Mobile",
                    DisplayOrder = 5
                }
            );
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "role-admin"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "Customer",
                    NormalizedName = "CUSTOMER",
                    ConcurrencyStamp = "role-customer"
                },
                new IdentityRole
                {
                    Id = "3",
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE",
                    ConcurrencyStamp = "role-employee"
                }
            );
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "admin-user-1",
                    RoleId = "1"
                }
            );

            builder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Wireless Headphones",
                    Description = "High-quality wireless headphones with noise cancellation and up to 30 hours of battery life.",
                    Price = 2499,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 2,
                    Name = "Smart LED TV",
                    Description = "55-inch 4K Smart TV with HDR support and built-in streaming applications.",
                    Price = 18999,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 3,
                    Name = "Bluetooth Speaker",
                    Description = "Portable Bluetooth speaker with powerful sound and up to 12 hours of battery life.",
                    Price = 1799,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 4,
                    Name = "Gaming Laptop",
                    Description = "High-performance gaming laptop with a powerful processor and dedicated graphics card.",
                    Price = 45999,
                    CategoryId = 2
                },
                new Product
                {
                    Id = 5,
                    Name = "Mechanical Keyboard",
                    Description = "RGB mechanical keyboard with blue switches, durable keys, and a compact design.",
                    Price = 1899,
                    CategoryId = 3
                },
                new Product
                {
                    Id = 6,
                    Name = "Wireless Mouse",
                    Description = "Ergonomic wireless mouse with adjustable DPI and long battery life.",
                    Price = 799,
                    CategoryId = 3
                },
                new Product
                {
                    Id = 7,
                    Name = "USB-C Hub",
                    Description = "Multi-port USB-C hub with HDMI, USB 3.0, and high-speed charging support.",
                    Price = 999,
                    CategoryId = 3
                },
                new Product
                {
                    Id = 11,
                    Name = "Smartphone",
                    Description = "Modern smartphone with a high-resolution display, powerful processor, and long-lasting battery.",
                    Price = 15999,
                    CategoryId = 5
                },
                new Product
                {
                    Id = 12,
                    Name = "Fast Charger",
                    Description = "Compact fast charger with USB-C Power Delivery support.",
                    Price = 699,
                    CategoryId = 5
                },
                new Product
                {
                     Id = 13,
                     Name = "Gaming Headset",
                     Description = "Surround-sound gaming headset with a clear microphone and comfortable ear cushions.",
                     Price = 2199,
                     CategoryId = 4
                },
                new Product
                {
                    Id = 14,
                    Name = "Gaming Mouse",
                    Description = "High-precision gaming mouse with adjustable DPI and customizable buttons.",
                    Price = 1299,
                    CategoryId = 4
                },
                new Product
                {
                    Id = 15,
                    Name = "Gaming Controller",
                    Description = "Wireless gaming controller with ergonomic design and responsive buttons.",
                    Price = 2499,
                    CategoryId = 4
                }
            );

        }

    }
}
