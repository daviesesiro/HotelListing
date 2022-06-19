﻿// <auto-generated />
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HotelListing.API.Migrations
{
    [DbContext(typeof(HotelListingContext))]
    partial class HotelListingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HotelListing.API.Data.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("ShortName")
                        .HasColumnType("text")
                        .HasColumnName("short_name");

                    b.HasKey("Id")
                        .HasName("pk_countries");

                    b.ToTable("countries", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "USA",
                            ShortName = "US"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Canada",
                            ShortName = "CA"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Mexico",
                            ShortName = "MX"
                        });
                });

            modelBuilder.Entity("HotelListing.API.Data.Hotel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("text")
                        .HasColumnName("address");

                    b.Property<int>("CountryId")
                        .HasColumnType("integer")
                        .HasColumnName("country_id");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<double>("Rating")
                        .HasColumnType("double precision")
                        .HasColumnName("rating");

                    b.HasKey("Id")
                        .HasName("pk_hotels");

                    b.HasIndex("CountryId")
                        .HasDatabaseName("ix_hotels_country_id");

                    b.ToTable("hotels", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "Geroge town",
                            CountryId = 1,
                            Name = "Hilton",
                            Rating = 4.5
                        },
                        new
                        {
                            Id = 2,
                            Address = "Manhattan",
                            CountryId = 3,
                            Name = "Sheraton",
                            Rating = 4.0
                        },
                        new
                        {
                            Id = 3,
                            Address = "Crazy zone",
                            CountryId = 2,
                            Name = "Hyatt",
                            Rating = 3.5
                        },
                        new
                        {
                            Id = 4,
                            Address = "Match Match street",
                            CountryId = 1,
                            Name = "Marriott",
                            Rating = 3.0
                        },
                        new
                        {
                            Id = 5,
                            Address = "Agege",
                            CountryId = 3,
                            Name = "Four Seasons",
                            Rating = 4.0
                        });
                });

            modelBuilder.Entity("HotelListing.API.Data.Hotel", b =>
                {
                    b.HasOne("HotelListing.API.Data.Country", "Country")
                        .WithMany("Hotels")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_hotels_countries_country_id");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("HotelListing.API.Data.Country", b =>
                {
                    b.Navigation("Hotels");
                });
#pragma warning restore 612, 618
        }
    }
}
