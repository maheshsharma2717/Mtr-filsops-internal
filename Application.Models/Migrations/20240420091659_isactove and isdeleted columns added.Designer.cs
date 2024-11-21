﻿// <auto-generated />
using System;
using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Application.Models.Migrations
{
    [DbContext(typeof(MtrContext))]
    [Migration("20240420091659_isactove and isdeleted columns added")]
    partial class isactoveandisdeletedcolumnsadded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Application.Models.Fieldo_RequestCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.ToTable("Fieldo_RequestCategory");
                });

            modelBuilder.Entity("Application.Models.Fieldo_Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Fieldo_Roles");
                });

            modelBuilder.Entity("Application.Models.Fieldo_ServiceRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Documents")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<decimal?>("OfferPrice")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("UpdatedBy");

                    b.ToTable("Fieldo_ServiceRequest");
                });

            modelBuilder.Entity("Application.Models.Fieldo_Task", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("AssignedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("AssignedBy")
                        .HasColumnType("int");

                    b.Property<int?>("AssignedTo")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("AssignedBy");

                    b.HasIndex("AssignedTo");

                    b.HasIndex("CreatedBy");

                    b.ToTable("Fieldo_Task");
                });

            modelBuilder.Entity("Application.Models.Fieldo_UserDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Fieldo_UserDetails");
                });

            modelBuilder.Entity("Application.Models.Fieldo_RequestCategory", b =>
                {
                    b.HasOne("Application.Models.Fieldo_UserDetails", "RequestCategoryCreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.Navigation("RequestCategoryCreatedBy");
                });

            modelBuilder.Entity("Application.Models.Fieldo_ServiceRequest", b =>
                {
                    b.HasOne("Application.Models.Fieldo_RequestCategory", "RequestCategoryCategoryId")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Models.Fieldo_UserDetails", "ServiceRequestCreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("Application.Models.Fieldo_UserDetails", "ServiceRequestUpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedBy");

                    b.Navigation("RequestCategoryCategoryId");

                    b.Navigation("ServiceRequestCreatedBy");

                    b.Navigation("ServiceRequestUpdatedBy");
                });

            modelBuilder.Entity("Application.Models.Fieldo_Task", b =>
                {
                    b.HasOne("Application.Models.Fieldo_UserDetails", "UserDetailsAssignedBy")
                        .WithMany()
                        .HasForeignKey("AssignedBy");

                    b.HasOne("Application.Models.Fieldo_UserDetails", "UserDetailsAssignedTo")
                        .WithMany()
                        .HasForeignKey("AssignedTo");

                    b.HasOne("Application.Models.Fieldo_UserDetails", "UserDetailsCreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.Navigation("UserDetailsAssignedBy");

                    b.Navigation("UserDetailsAssignedTo");

                    b.Navigation("UserDetailsCreatedBy");
                });

            modelBuilder.Entity("Application.Models.Fieldo_UserDetails", b =>
                {
                    b.HasOne("Application.Models.Fieldo_Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });
#pragma warning restore 612, 618
        }
    }
}
