﻿// <auto-generated />
using System;
using EventPlatFormVer4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EventPlatFormVer4.Migrations
{
    [DbContext(typeof(MvcEpfContext))]
    [Migration("20201021075307_migrations3")]
    partial class migrations3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("EventPlatFormVer4.Models.Administrator", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Pwd")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<uint>("RoleID")
                        .HasColumnType("int unsigned");

                    b.HasKey("Id");

                    b.ToTable("Administrators");
                });

            modelBuilder.Entity("EventPlatFormVer4.Models.Event", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned");

                    b.Property<string>("Address")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Detail")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("EventEndTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("EventStartTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Rank")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("SignUpEndTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("SignUpStartTime")
                        .HasColumnType("datetime(6)");

                    b.Property<uint?>("SponsorId")
                        .HasColumnType("int unsigned");

                    b.HasKey("Id");

                    b.HasIndex("SponsorId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("EventPlatFormVer4.Models.Participatant", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<uint?>("EventId")
                        .HasColumnType("int unsigned");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Pwd")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<uint>("RoleID")
                        .HasColumnType("int unsigned");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("Participatants");
                });

            modelBuilder.Entity("EventPlatFormVer4.Models.Person", b =>
                {
                    b.Property<uint>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Pwd")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<uint>("RoleID")
                        .HasColumnType("int unsigned");

                    b.HasKey("ID");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("EventPlatFormVer4.Models.Sponsor", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned");

                    b.Property<string>("Certificate")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Pwd")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<uint>("RoleID")
                        .HasColumnType("int unsigned");

                    b.HasKey("Id");

                    b.ToTable("Sponsors");
                });

            modelBuilder.Entity("EventPlatFormVer4.Models.Event", b =>
                {
                    b.HasOne("EventPlatFormVer4.Models.Sponsor", "Sponsor")
                        .WithMany()
                        .HasForeignKey("SponsorId");
                });

            modelBuilder.Entity("EventPlatFormVer4.Models.Participatant", b =>
                {
                    b.HasOne("EventPlatFormVer4.Models.Event", null)
                        .WithMany("Participants")
                        .HasForeignKey("EventId");
                });
#pragma warning restore 612, 618
        }
    }
}
