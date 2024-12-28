﻿// <auto-generated />
using AirportDictionaryApp_v1.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AirportDictionaryApp_v1.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AirportCompany", b =>
                {
                    b.Property<int>("AirportsId")
                        .HasColumnType("int");

                    b.Property<int>("CompaniesId")
                        .HasColumnType("int");

                    b.HasKey("AirportsId", "CompaniesId");

                    b.HasIndex("CompaniesId");

                    b.ToTable("AirportCompany");
                });

            modelBuilder.Entity("AirportDictionaryApp_v1.Model.Airport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<long>("AnnualPassengerTraffic")
                        .HasColumnType("bigint");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OpeningYear")
                        .HasColumnType("int");

                    b.Property<int>("RunwayCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("CountryId");

                    b.ToTable("Airports");
                });

            modelBuilder.Entity("AirportDictionaryApp_v1.Model.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("AirportDictionaryApp_v1.Model.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("AirportCompany", b =>
                {
                    b.HasOne("AirportDictionaryApp_v1.Model.Airport", null)
                        .WithMany()
                        .HasForeignKey("AirportsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AirportDictionaryApp_v1.Model.Company", null)
                        .WithMany()
                        .HasForeignKey("CompaniesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AirportDictionaryApp_v1.Model.Airport", b =>
                {
                    b.HasOne("AirportDictionaryApp_v1.Model.Country", "Country")
                        .WithMany("Airports")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("AirportDictionaryApp_v1.Model.Country", b =>
                {
                    b.Navigation("Airports");
                });
#pragma warning restore 612, 618
        }
    }
}
