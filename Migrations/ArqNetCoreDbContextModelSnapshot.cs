﻿// <auto-generated />
using System;
using ArqNetCore.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ArqNetCore.Migrations
{
    [DbContext(typeof(ArqNetCoreDbContext))]
    partial class ArqNetCoreDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ArqNetCore.Entities.Account", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(767)");

                    b.Property<bool>("Enable")
                        .HasColumnType("bit");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(4000)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(4000)");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("ArqNetCore.Entities.Area", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.HasKey("Name");

                    b.ToTable("Areas");

                    b.HasData(
                        new
                        {
                            Name = "ATENCION_A_PACIENTES",
                            Description = "Atención de pacientes"
                        },
                        new
                        {
                            Name = "TERAPIA_INTENSIVA",
                            Description = "Terapia Intensiva"
                        },
                        new
                        {
                            Name = "TECNICOS",
                            Description = "Técnicos"
                        });
                });

            modelBuilder.Entity("ArqNetCore.Entities.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Organizations");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Insumos Basicos"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Insumos mecanicos"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Medicamentos"
                        });
                });

            modelBuilder.Entity("ArqNetCore.Entities.OrganizationSupplyType", b =>
                {
                    b.Property<string>("SupplyTypeId")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32);

                    b.Property<int>("OrganizationId")
                        .HasColumnType("int");

                    b.HasKey("SupplyTypeId", "OrganizationId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("OrganizationSupplyTypes");

                    b.HasData(
                        new
                        {
                            SupplyTypeId = "BARBIJO",
                            OrganizationId = 1
                        },
                        new
                        {
                            SupplyTypeId = "GUANTE",
                            OrganizationId = 1
                        },
                        new
                        {
                            SupplyTypeId = "MASCARA_PROTECTORA",
                            OrganizationId = 1
                        },
                        new
                        {
                            SupplyTypeId = "RESPIRADOR",
                            OrganizationId = 2
                        },
                        new
                        {
                            SupplyTypeId = "MEDICAMENTO",
                            OrganizationId = 3
                        });
                });

            modelBuilder.Entity("ArqNetCore.Entities.SuppliesOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AccountId")
                        .HasColumnType("varchar(767)");

                    b.Property<string>("AreaId")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32);

                    b.Property<int?>("OrganizationId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("AreaId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("SuppliesOrders");
                });

            modelBuilder.Entity("ArqNetCore.Entities.Supply", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("SuppliesOrderId")
                        .HasColumnType("int");

                    b.Property<string>("SupplyTypeId")
                        .HasColumnType("varchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("SuppliesOrderId");

                    b.HasIndex("SupplyTypeId");

                    b.ToTable("Supplies");
                });

            modelBuilder.Entity("ArqNetCore.Entities.SupplyAttribute", b =>
                {
                    b.Property<int>("SupplyId")
                        .HasColumnType("int");

                    b.Property<string>("AttributeName")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("AttributeValue")
                        .HasColumnType("text");

                    b.HasKey("SupplyId", "AttributeName");

                    b.ToTable("SupplyAttributes");
                });

            modelBuilder.Entity("ArqNetCore.Entities.SupplyType", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SupplyTypes");

                    b.HasData(
                        new
                        {
                            Id = "MASCARA_PROTECTORA",
                            Description = "Máscara protectora"
                        },
                        new
                        {
                            Id = "BARBIJO",
                            Description = "Barbijo"
                        },
                        new
                        {
                            Id = "RESPIRADOR",
                            Description = "Respirador"
                        },
                        new
                        {
                            Id = "GUANTE",
                            Description = "Guante"
                        },
                        new
                        {
                            Id = "MEDICAMENTO",
                            Description = "Medicamento"
                        });
                });

            modelBuilder.Entity("ArqNetCore.Entities.SupplyTypeAttribute", b =>
                {
                    b.Property<string>("SupplyTypeId")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("AttributeName")
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("AttributeDescription")
                        .HasColumnType("text");

                    b.HasKey("SupplyTypeId", "AttributeName");

                    b.ToTable("SupplyTypeAttributes");

                    b.HasData(
                        new
                        {
                            SupplyTypeId = "MEDICAMENTO",
                            AttributeName = "TIPO_MEDICAMENTO",
                            AttributeDescription = "Tipo de medicamento"
                        });
                });

            modelBuilder.Entity("ArqNetCore.Entities.User", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("varchar(767)");

                    b.Property<string>("Company")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("Locality")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<string>("Position")
                        .HasColumnType("text");

                    b.HasKey("Email");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ArqNetCore.Entities.OrganizationSupplyType", b =>
                {
                    b.HasOne("ArqNetCore.Entities.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArqNetCore.Entities.SupplyType", "SupplyType")
                        .WithMany()
                        .HasForeignKey("SupplyTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ArqNetCore.Entities.SuppliesOrder", b =>
                {
                    b.HasOne("ArqNetCore.Entities.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId");

                    b.HasOne("ArqNetCore.Entities.Area", "Area")
                        .WithMany()
                        .HasForeignKey("AreaId");

                    b.HasOne("ArqNetCore.Entities.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("ArqNetCore.Entities.Supply", b =>
                {
                    b.HasOne("ArqNetCore.Entities.SuppliesOrder", "SuppliesOrder")
                        .WithMany()
                        .HasForeignKey("SuppliesOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArqNetCore.Entities.SupplyType", "SupplyType")
                        .WithMany()
                        .HasForeignKey("SupplyTypeId");
                });

            modelBuilder.Entity("ArqNetCore.Entities.SupplyAttribute", b =>
                {
                    b.HasOne("ArqNetCore.Entities.Supply", "Supply")
                        .WithMany()
                        .HasForeignKey("SupplyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ArqNetCore.Entities.SupplyTypeAttribute", b =>
                {
                    b.HasOne("ArqNetCore.Entities.SupplyType", "SupplyType")
                        .WithMany()
                        .HasForeignKey("SupplyTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ArqNetCore.Entities.User", b =>
                {
                    b.HasOne("ArqNetCore.Entities.Account", "Account")
                        .WithMany()
                        .HasForeignKey("Email")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}