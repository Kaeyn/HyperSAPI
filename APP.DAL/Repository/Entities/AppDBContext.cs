using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace APP.DAL.Repository.Entities;

public partial class AppDBContext : DbContext
{
    public AppDBContext()
    {
    }

    public AppDBContext(DbContextOptions<AppDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<EfmigrationsHistory> EfmigrationsHistories { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductSize> ProductSizes { get; set; }

    public virtual DbSet<ShippingAddress> ShippingAddresses { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");
        optionsBuilder.UseMySql("server=hyperssql-cakhosolo2003-325a.e.aivencloud.com;port=17997;database=defaultdb;user=avnadmin;password=AVNS_EBxOtAQ6lHdDe2fbQEh;sslmode=Required", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.30-mysql"));
    }
           
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("Brand");

            entity.HasIndex(e => e.Code, "Code_UNIQUE").IsUnique();

            entity.Property(e => e.BrandName).HasMaxLength(45);
            entity.Property(e => e.IdBrand).HasMaxLength(45);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryName).HasMaxLength(45);
            entity.Property(e => e.IdCategory).HasMaxLength(45);
            entity.Property(e => e.ParentId).HasMaxLength(45);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.CodeUser, "FkCustomer_User_IdUser_idx");

            entity.Property(e => e.Avartar).HasMaxLength(45);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.CodeUserNavigation).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CodeUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkCustomer_User_CodeUser");
        });

        modelBuilder.Entity<EfmigrationsHistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

            entity.ToTable("__EFMigrationsHistory");

            entity.Property(e => e.MigrationId).HasMaxLength(95);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("Employee");

            entity.HasIndex(e => e.CodeUser, "FkEmployee_User_CodeUser_idx");

            entity.Property(e => e.Avartar).HasMaxLength(45);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.CodeUserNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.CodeUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkEmployee_User_CodeUser");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("Product");

            entity.HasIndex(e => e.IdProduct, "IdProduct_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Brand, "brand_idx").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.IdProduct).HasMaxLength(45);
            entity.Property(e => e.ProductName).HasMaxLength(45);
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("ProductImage");

            entity.HasIndex(e => e.ProductCode, "FkProductImage_Product_ProductCode_idx");

            entity.Property(e => e.IdImage).HasMaxLength(10);
            entity.Property(e => e.Img)
                .HasMaxLength(200)
                .HasColumnName("IMG");

            entity.HasOne(d => d.ProductCodeNavigation).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkProductImage_Product_ProductCode");
        });

        modelBuilder.Entity<ProductSize>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("ProductSize");

            entity.HasIndex(e => e.CodeProduct, "FkProductSize_Product_CodeProduct_idx");

            entity.HasIndex(e => e.CodeSize, "FkProductSize_Size_CodeSize_idx");

            entity.Property(e => e.Code).ValueGeneratedNever();

            entity.HasOne(d => d.CodeProductNavigation).WithMany(p => p.ProductSizes)
                .HasForeignKey(d => d.CodeProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkProductSize_Product_CodeProduct");

            entity.HasOne(d => d.CodeSizeNavigation).WithMany(p => p.ProductSizes)
                .HasForeignKey(d => d.CodeSize)
                .HasConstraintName("FkProductSize_Size_CodeSize");
        });

        modelBuilder.Entity<ShippingAddress>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("ShippingAddress");

            entity.HasIndex(e => e.CustomerCode, "FkShippingAddress_Customer_CustomerCode_idx");

            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.AddressNote).HasMaxLength(200);
            entity.Property(e => e.IsDefaultAddress).HasComment("0: False\n1: True");
            entity.Property(e => e.PhoneNumber).HasMaxLength(45);
            entity.Property(e => e.ReceiverName).HasMaxLength(50);

            entity.HasOne(d => d.CustomerCodeNavigation).WithMany(p => p.ShippingAddresses)
                .HasForeignKey(d => d.CustomerCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkShippingAddress_Customer_CustomerCode");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("Size");

            entity.HasIndex(e => e.Code, "Code_UNIQUE").IsUnique();

            entity.Property(e => e.IdSize).HasMaxLength(10);
            entity.Property(e => e.Size1).HasColumnName("Size");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("User");

            entity.HasIndex(e => e.IdUser, "idUser_UNIQUE").IsUnique();

            entity.Property(e => e.Avatar).HasMaxLength(45);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Gender).HasComment("0: Male;\n1: Female;");
            entity.Property(e => e.IdUser).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Permission).HasComment("0: Customer;\n1: Admin;\n2: Staff;");
            entity.Property(e => e.PhoneNumber).HasMaxLength(13);
            entity.Property(e => e.Status).HasComment("0: Normal;\n1: Blocked;");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
