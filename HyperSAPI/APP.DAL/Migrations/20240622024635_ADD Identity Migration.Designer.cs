﻿// <auto-generated />
using System;
using APP.DAL.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace APP.DAL.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20240622024635_ADD Identity Migration")]
    partial class ADDIdentityMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");
            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("APP.DAL.Repository.Entities.Bill", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Code"));

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<int>("PaymentMethod")
                        .HasColumnType("int")
                        .HasComment("0: COD\n1: QR\n");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("varchar(13)");

                    b.Property<string>("ShippingAddress")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("TotalBill")
                        .HasColumnType("int");

                    b.HasKey("Code")
                        .HasName("PRIMARY");

                    b.ToTable("Bill", (string)null);
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.BillInfo", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Code"));

                    b.Property<string>("TotalPrice")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<int>("CodeBill")
                        .HasColumnType("int");

                    b.Property<int>("CodeProduct")
                        .HasColumnType("int");

                    b.Property<int?>("Discount")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Code", "TotalPrice")
                        .HasName("PRIMARY")
                        .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                    b.HasIndex(new[] { "CodeBill" }, "FkBillInfo_Bill_CodeBill_idx");

                    b.HasIndex(new[] { "CodeProduct" }, "FkBillInfo_Product_CodeProduct_idx");

                    b.ToTable("BillInfo", (string)null);
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.Brand", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Code"));

                    b.Property<string>("BrandName")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("IdBrand")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("ImageURL");

                    b.HasKey("Code")
                        .HasName("PRIMARY");

                    b.ToTable("Brand", (string)null);
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.Cart", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Code"));

                    b.Property<int>("CodeCustomer")
                        .HasColumnType("int");

                    b.Property<int>("CodeProduct")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("SelectedSize")
                        .HasColumnType("int");

                    b.HasKey("Code")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "CodeCustomer" }, "FkCart_Customer_CodeCustomer_idx");

                    b.HasIndex(new[] { "CodeProduct" }, "FkCart_Product_CodeProduct_idx");

                    b.ToTable("Cart", (string)null);
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.Category", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Code"));

                    b.Property<string>("CategoryName")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("IdCategory")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("ParentId")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.HasKey("Code")
                        .HasName("PRIMARY");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.Customer", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Code"));

                    b.Property<DateOnly?>("Birthday")
                        .HasColumnType("date");

                    b.Property<int>("CodeUser")
                        .HasColumnType("int");

                    b.Property<int?>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Idcustomer")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("IDCustomer");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("ImageURL");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Code")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "CodeUser" }, "FkCustomer_User_IdUser_idx");

                    b.ToTable("Customer", (string)null);
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.EfmigrationsHistory", b =>
                {
                    b.Property<string>("MigrationId")
                        .HasMaxLength(95)
                        .HasColumnType("varchar(95)");

                    b.Property<string>("ProductVersion")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.HasKey("MigrationId")
                        .HasName("PRIMARY");

                    b.ToTable("__EFMigrationsHistory", (string)null);
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.Permission", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Code"));

                    b.Property<string>("IdPermission")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("PermissionName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.HasKey("Code")
                        .HasName("PRIMARY");

                    b.ToTable("Permission", (string)null);
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.Position", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Code"));

                    b.Property<string>("IdPosition")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("PositionName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.HasKey("Code")
                        .HasName("PRIMARY");

                    b.ToTable("Position", (string)null);
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.Product", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Code"));

                    b.Property<int>("Brand")
                        .HasColumnType("int");

                    b.Property<string>("Color")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("Description")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<int?>("Discount")
                        .HasColumnType("int");

                    b.Property<string>("DiscountDescription")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int?>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("IdProduct")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<int?>("IsNew")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("'1'")
                        .HasComment("0: Normal\n1: New");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<int>("ProductType")
                        .HasColumnType("int");

                    b.Property<int?>("Sold")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int?>("Stock")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("'0'");

                    b.HasKey("Code")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "ProductType" }, "FkProduct_ProductType_CodeProductType_idx");

                    b.HasIndex(new[] { "IdProduct" }, "IdProduct_UNIQUE")
                        .IsUnique();

                    b.HasIndex(new[] { "Brand" }, "brand_idx");

                    b.ToTable("Product", (string)null);
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.ProductImage", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Code"));

                    b.Property<string>("IdImage")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Img")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)")
                        .HasColumnName("IMG");

                    b.Property<sbyte>("IsThumbnail")
                        .HasColumnType("tinyint")
                        .HasComment("0: FALSE\n1: TRUE");

                    b.Property<int>("ProductCode")
                        .HasColumnType("int");

                    b.HasKey("Code")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "ProductCode" }, "FkProductImage_Product_ProductCode_idx");

                    b.HasIndex(new[] { "IdImage" }, "IdImage_UNIQUE")
                        .IsUnique();

                    b.ToTable("ProductImage", (string)null);
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.ProductSize", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Code"));

                    b.Property<int>("CodeProduct")
                        .HasColumnType("int");

                    b.Property<int>("CodeSize")
                        .HasColumnType("int");

                    b.HasKey("Code")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "CodeProduct" }, "FkProductSize_Product_CodeProduct_idx");

                    b.HasIndex(new[] { "CodeSize" }, "FkProductSize_Size_CodeSize_idx");

                    b.ToTable("ProductSize", (string)null);
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.ProductType", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Code"));

                    b.Property<string>("IdProductType")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.HasKey("Code")
                        .HasName("PRIMARY");

                    b.ToTable("ProductType", (string)null);
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.ShippingAddress", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Code"));

                    b.Property<string>("Address")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("AddressNote")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<int>("CustomerCode")
                        .HasColumnType("int");

                    b.Property<int?>("IsDefaultAddress")
                        .HasColumnType("int")
                        .HasComment("0: False\n1: True");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("ReceiverName")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Code")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "CustomerCode" }, "FkShippingAddress_Customer_CustomerCode_idx");

                    b.ToTable("ShippingAddress", (string)null);
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.Size", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Code"));

                    b.Property<string>("IdSize")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<int>("Size1")
                        .HasColumnType("int")
                        .HasColumnName("Size");

                    b.HasKey("Code")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Code" }, "Code_UNIQUE")
                        .IsUnique();

                    b.ToTable("Size", (string)null);
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.Staff", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Code"));

                    b.Property<string>("Address")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateOnly?>("Birthday")
                        .HasColumnType("date");

                    b.Property<int>("CodeUser")
                        .HasColumnType("int");

                    b.Property<int?>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Identication")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<string>("Idstaff")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("IDStaff");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("ImageURL");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.HasKey("Code")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Position" }, "FkStaff_Position_CodePosition_idx");

                    b.HasIndex(new[] { "CodeUser" }, "FkStaff_User_CodeUser_idx");

                    b.ToTable("Staff");
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.User", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Code"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("IdUser")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<int>("Permission")
                        .HasColumnType("int")
                        .HasComment("0: Customer;\\n1: Admin;\\n2: Staff;");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("varchar(13)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasComment("0: Normal;\\n1: Blocked;");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Code")
                        .HasName("PRIMARY");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex(new[] { "Email" }, "Email_UNIQUE")
                        .IsUnique();

                    b.HasIndex(new[] { "Permission" }, "FkUser_Permission_CodePermission_idx");

                    b.HasIndex(new[] { "PhoneNumber" }, "PhoneNumber_UNIQUE")
                        .IsUnique();

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.BillInfo", b =>
                {
                    b.HasOne("APP.DAL.Repository.Entities.Bill", "CodeBillNavigation")
                        .WithMany("BillInfos")
                        .HasForeignKey("CodeBill")
                        .IsRequired()
                        .HasConstraintName("FkBillInfo_Bill_CodeBill");

                    b.HasOne("APP.DAL.Repository.Entities.Product", "CodeProductNavigation")
                        .WithMany("BillInfos")
                        .HasForeignKey("CodeProduct")
                        .IsRequired()
                        .HasConstraintName("FkBillInfo_Product_CodeProduct");

                    b.Navigation("CodeBillNavigation");

                    b.Navigation("CodeProductNavigation");
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.Cart", b =>
                {
                    b.HasOne("APP.DAL.Repository.Entities.Customer", "CodeCustomerNavigation")
                        .WithMany("Carts")
                        .HasForeignKey("CodeCustomer")
                        .IsRequired()
                        .HasConstraintName("FkCart_Customer_CodeCustomer");

                    b.HasOne("APP.DAL.Repository.Entities.Product", "CodeProductNavigation")
                        .WithMany("Carts")
                        .HasForeignKey("CodeProduct")
                        .IsRequired()
                        .HasConstraintName("FkCart_Product_CodeProduct");

                    b.Navigation("CodeCustomerNavigation");

                    b.Navigation("CodeProductNavigation");
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.Customer", b =>
                {
                    b.HasOne("APP.DAL.Repository.Entities.User", "CodeUserNavigation")
                        .WithMany("Customers")
                        .HasForeignKey("CodeUser")
                        .IsRequired()
                        .HasConstraintName("FkCustomer_User_CodeUser");

                    b.Navigation("CodeUserNavigation");
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.Product", b =>
                {
                    b.HasOne("APP.DAL.Repository.Entities.Brand", "BrandNavigation")
                        .WithMany("Products")
                        .HasForeignKey("Brand")
                        .IsRequired()
                        .HasConstraintName("FkProduct_Brand_CodeBrand");

                    b.HasOne("APP.DAL.Repository.Entities.ProductType", "ProductTypeNavigation")
                        .WithMany("Products")
                        .HasForeignKey("ProductType")
                        .IsRequired()
                        .HasConstraintName("FkProduct_ProductType_CodeProductType");

                    b.Navigation("BrandNavigation");

                    b.Navigation("ProductTypeNavigation");
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.ProductImage", b =>
                {
                    b.HasOne("APP.DAL.Repository.Entities.Product", "ProductCodeNavigation")
                        .WithMany("ProductImages")
                        .HasForeignKey("ProductCode")
                        .IsRequired()
                        .HasConstraintName("FkProductImage_Product_ProductCode");

                    b.Navigation("ProductCodeNavigation");
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.ProductSize", b =>
                {
                    b.HasOne("APP.DAL.Repository.Entities.Product", "CodeProductNavigation")
                        .WithMany("ProductSizes")
                        .HasForeignKey("CodeProduct")
                        .IsRequired()
                        .HasConstraintName("FkProductSize_Product_CodeProduct");

                    b.HasOne("APP.DAL.Repository.Entities.Size", "CodeSizeNavigation")
                        .WithMany("ProductSizes")
                        .HasForeignKey("CodeSize")
                        .IsRequired()
                        .HasConstraintName("FkProductSize_Size_CodeSize");

                    b.Navigation("CodeProductNavigation");

                    b.Navigation("CodeSizeNavigation");
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.ShippingAddress", b =>
                {
                    b.HasOne("APP.DAL.Repository.Entities.Customer", "CustomerCodeNavigation")
                        .WithMany("ShippingAddresses")
                        .HasForeignKey("CustomerCode")
                        .IsRequired()
                        .HasConstraintName("FkShippingAddress_Customer_CustomerCode");

                    b.Navigation("CustomerCodeNavigation");
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.Staff", b =>
                {
                    b.HasOne("APP.DAL.Repository.Entities.User", "CodeUserNavigation")
                        .WithMany("Staff")
                        .HasForeignKey("CodeUser")
                        .IsRequired()
                        .HasConstraintName("FkStaff_User_CodeUser");

                    b.HasOne("APP.DAL.Repository.Entities.Position", "PositionNavigation")
                        .WithMany("Staff")
                        .HasForeignKey("Position")
                        .IsRequired()
                        .HasConstraintName("FkStaff_Position_CodePosition");

                    b.Navigation("CodeUserNavigation");

                    b.Navigation("PositionNavigation");
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.User", b =>
                {
                    b.HasOne("APP.DAL.Repository.Entities.Permission", "PermissionNavigation")
                        .WithMany("Users")
                        .HasForeignKey("Permission")
                        .IsRequired()
                        .HasConstraintName("FkUser_Permission_CodePermission");

                    b.Navigation("PermissionNavigation");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("APP.DAL.Repository.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("APP.DAL.Repository.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APP.DAL.Repository.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("APP.DAL.Repository.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.Bill", b =>
                {
                    b.Navigation("BillInfos");
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.Brand", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.Customer", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("ShippingAddresses");
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.Permission", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.Position", b =>
                {
                    b.Navigation("Staff");
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.Product", b =>
                {
                    b.Navigation("BillInfos");

                    b.Navigation("Carts");

                    b.Navigation("ProductImages");

                    b.Navigation("ProductSizes");
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.ProductType", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.Size", b =>
                {
                    b.Navigation("ProductSizes");
                });

            modelBuilder.Entity("APP.DAL.Repository.Entities.User", b =>
                {
                    b.Navigation("Customers");

                    b.Navigation("Staff");
                });
#pragma warning restore 612, 618
        }
    }
}
