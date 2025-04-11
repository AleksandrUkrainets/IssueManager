﻿// <auto-generated />
using System;
using IssueManager.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IssueManager.Persistance.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("IssueManager.Domain.Entities.DB.UserCredential", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AccessTokenEncrypted")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("AppUserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("JwtTokenEncrypted")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Provider")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId", "Provider")
                        .IsUnique();

                    b.ToTable("UserCredentials");
                });
#pragma warning restore 612, 618
        }
    }
}
