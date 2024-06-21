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

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<BillInfo> BillInfos { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<EfmigrationsHistory> EfmigrationsHistories { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductSize> ProductSizes { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<ShippingAddress> ShippingAddresses { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");
        if (connectionString == null)
        {
            connectionString = "Server=hyperssql-cakhosolo2003-325a.e.aivencloud.com;Port=17997;Database=defaultdb;User=avnadmin;Password=AVNS_EBxOtAQ6lHdDe2fbQEh;SslMode=Required;";
        }
        optionsBuilder.UseMySql(connectionString, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.30-mysql"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("Bill");

            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.CustomerName).HasMaxLength(45);
            entity.Property(e => e.PaymentMethod).HasComment("0: COD\n1: QR\n");
            entity.Property(e => e.PhoneNumber).HasMaxLength(13);
            entity.Property(e => e.ShippingAddress).HasMaxLength(255);
        });

        modelBuilder.Entity<BillInfo>(entity =>
        {
            entity.HasKey(e => new { e.Code, e.TotalPrice })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("BillInfo");

            entity.HasIndex(e => e.CodeBill, "FkBillInfo_Bill_CodeBill_idx");

            entity.HasIndex(e => e.CodeProduct, "FkBillInfo_Product_CodeProduct_idx");

            entity.Property(e => e.Code).ValueGeneratedOnAdd();
            entity.Property(e => e.TotalPrice).HasMaxLength(45);

            entity.HasOne(d => d.CodeBillNavigation).WithMany(p => p.BillInfos)
                .HasForeignKey(d => d.CodeBill)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkBillInfo_Bill_CodeBill");

            entity.HasOne(d => d.CodeProductNavigation).WithMany(p => p.BillInfos)
                .HasForeignKey(d => d.CodeProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkBillInfo_Product_CodeProduct");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("Brand");

            entity.Property(e => e.BrandName).HasMaxLength(45);
            entity.Property(e => e.IdBrand).HasMaxLength(45);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(100)
                .HasColumnName("ImageURL");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("Cart");

            entity.HasIndex(e => e.CodeCustomer, "FkCart_Customer_CodeCustomer_idx");

            entity.HasIndex(e => e.CodeProduct, "FkCart_Product_CodeProduct_idx");

            entity.HasOne(d => d.CodeCustomerNavigation).WithMany(p => p.Carts)
                .HasForeignKey(d => d.CodeCustomer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkCart_Customer_CodeCustomer");

            entity.HasOne(d => d.CodeProductNavigation).WithMany(p => p.Carts)
                .HasForeignKey(d => d.CodeProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkCart_Product_CodeProduct");
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

            entity.Property(e => e.Idcustomer)
                .HasMaxLength(100)
                .HasColumnName("IDCustomer");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(100)
                .HasColumnName("ImageURL");
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

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("Permission");

            entity.Property(e => e.IdPermission).HasMaxLength(45);
            entity.Property(e => e.PermissionName).HasMaxLength(45);
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("Position");

            entity.Property(e => e.IdPosition).HasMaxLength(45);
            entity.Property(e => e.PositionName).HasMaxLength(45);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("Product");

            entity.HasIndex(e => e.ProductType, "FkProduct_ProductType_CodeProductType_idx");

            entity.HasIndex(e => e.IdProduct, "IdProduct_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Brand, "brand_idx");

            entity.Property(e => e.Color).HasMaxLength(45);
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.DiscountDescription).HasMaxLength(100);
            entity.Property(e => e.IdProduct).HasMaxLength(45);
            entity.Property(e => e.IsNew)
                .HasDefaultValueSql("'1'")
                .HasComment("0: Normal\n1: New");
            entity.Property(e => e.ProductName).HasMaxLength(45);
            entity.Property(e => e.Stock).HasDefaultValueSql("'0'");

            entity.HasOne(d => d.BrandNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Brand)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkProduct_Brand_CodeBrand");

            entity.HasOne(d => d.ProductTypeNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkProduct_ProductType_CodeProductType");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("ProductImage");

            entity.HasIndex(e => e.ProductCode, "FkProductImage_Product_ProductCode_idx");

            entity.HasIndex(e => e.IdImage, "IdImage_UNIQUE").IsUnique();

            entity.Property(e => e.IdImage).HasMaxLength(10);
            entity.Property(e => e.Img)
                .HasMaxLength(500)
                .HasColumnName("IMG");
            entity.Property(e => e.IsThumbnail).HasComment("0: FALSE\n1: TRUE");

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

            entity.HasOne(d => d.CodeProductNavigation).WithMany(p => p.ProductSizes)
                .HasForeignKey(d => d.CodeProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkProductSize_Product_CodeProduct");

            entity.HasOne(d => d.CodeSizeNavigation).WithMany(p => p.ProductSizes)
                .HasForeignKey(d => d.CodeSize)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkProductSize_Size_CodeSize");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("ProductType");

            entity.Property(e => e.IdProductType).HasMaxLength(45);
            entity.Property(e => e.Name).HasMaxLength(45);
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

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.HasIndex(e => e.Position, "FkStaff_Position_CodePosition_idx");

            entity.HasIndex(e => e.CodeUser, "FkStaff_User_CodeUser_idx");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Identication).HasMaxLength(45);
            entity.Property(e => e.Idstaff)
                .HasMaxLength(100)
                .HasColumnName("IDStaff");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(100)
                .HasColumnName("ImageURL");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.CodeUserNavigation).WithMany(p => p.Staff)
                .HasForeignKey(d => d.CodeUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkStaff_User_CodeUser");

            entity.HasOne(d => d.PositionNavigation).WithMany(p => p.Staff)
                .HasForeignKey(d => d.Position)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkStaff_Position_CodePosition");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "Email_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Permission, "FkUser_Permission_CodePermission_idx");

            entity.HasIndex(e => e.PhoneNumber, "PhoneNumber_UNIQUE").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.IdUser).HasMaxLength(10);
            entity.Property(e => e.Permission).HasComment("0: Customer;\\n1: Admin;\\n2: Staff;");
            entity.Property(e => e.PhoneNumber).HasMaxLength(13);
            entity.Property(e => e.Status).HasComment("0: Normal;\\n1: Blocked;");

            entity.HasOne(d => d.PermissionNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Permission)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkUser_Permission_CodePermission");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
