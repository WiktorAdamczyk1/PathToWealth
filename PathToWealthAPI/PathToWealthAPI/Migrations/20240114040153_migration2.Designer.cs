﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PathToWealthAPI.Data;

#nullable disable

namespace PathToWealthAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240114040153_migration2")]
    partial class migration2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PathToWealthAPI.Data.Models+RefreshToken", b =>
                {
                    b.Property<int>("RefreshTokenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RefreshTokenId"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Revoked")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("RefreshTokenId");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("PathToWealthAPI.Data.Models+User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("PathToWealthAPI.Data.Models+UserFinancialData", b =>
                {
                    b.Property<int>("DataId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DataId"));

                    b.Property<decimal>("BondAnnualReturn")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("BondCostRatio")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("InitialInvestment")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsInvestmentMonthly")
                        .HasColumnType("bit");

                    b.Property<int>("RetirementDuration")
                        .HasColumnType("int");

                    b.Property<int>("StartInvestementYear")
                        .HasColumnType("int");

                    b.Property<int>("StartWithdrawalYear")
                        .HasColumnType("int");

                    b.Property<decimal>("StockAnnualReturn")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("StockCostRatio")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("StockToBondRatio")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<decimal>("YearlyOrMonthlySavings")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("DataId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserFinancialData");
                });

            modelBuilder.Entity("PathToWealthAPI.Data.Models+RefreshToken", b =>
                {
                    b.HasOne("PathToWealthAPI.Data.Models+User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PathToWealthAPI.Data.Models+UserFinancialData", b =>
                {
                    b.HasOne("PathToWealthAPI.Data.Models+User", "User")
                        .WithOne("UserFinancialData")
                        .HasForeignKey("PathToWealthAPI.Data.Models+UserFinancialData", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PathToWealthAPI.Data.Models+User", b =>
                {
                    b.Navigation("RefreshTokens");

                    b.Navigation("UserFinancialData")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}