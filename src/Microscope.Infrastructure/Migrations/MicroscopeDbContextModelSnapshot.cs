﻿// <auto-generated />
using System;
using Microscope.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Microscope.Infrastructure.Migrations
{
    [DbContext(typeof(MicroscopeDbContext))]
    partial class MicroscopeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("mcsp_common")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Microscope.Domain.Entities.Analytic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Dimension")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Analytics");
                });

            modelBuilder.Entity("Microscope.Domain.Entities.RemoteConfig", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Dimension")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("RemoteConfigs");
                });
#pragma warning restore 612, 618
        }
    }
}
