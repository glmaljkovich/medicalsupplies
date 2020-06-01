﻿// <auto-generated />
using ArqNetCore.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ArqNetCore.Migrations
{
    [DbContext(typeof(ArqNetCoreDbContext))]
    [Migration("20200525042453_CreateAccounts")]
    partial class CreateAccounts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
                }
            );
          
#pragma warning restore 612, 618
        }
    }
}
