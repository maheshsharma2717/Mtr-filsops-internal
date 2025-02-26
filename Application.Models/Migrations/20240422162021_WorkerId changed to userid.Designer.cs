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
    [Migration("20240422162021_WorkerId changed to userid")]
    partial class WorkerIdchangedtouserid
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Application.Models.Fieldo_Bank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Fieldo_Banks");
                });

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

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("AssignedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("AssignedBy")
                        .HasColumnType("int");

                    b.Property<int?>("AssignedTo")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("Price")
                        .HasColumnType("double");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AssignedBy");

                    b.HasIndex("AssignedTo");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("UpdatedBy");

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

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<float>("YearOfExperience")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Fieldo_UserDetails");
                });

            modelBuilder.Entity("Application.Models.Fieldo_WorkerBankDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("AccountType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("BankId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName2")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("OtherBankName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("RoutingNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BankId");

                    b.HasIndex("UserId");

                    b.ToTable("Fieldo_WorkerBankDetails");
                });

            modelBuilder.Entity("Application.Models.Fieldo_WorkerTasks", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AssignedBy")
                        .HasColumnType("int");

                    b.Property<int>("AssignedTo")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("TaskId")
                        .HasColumnType("int");

                    b.Property<string>("TaskStatus")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedDated")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("AssignedBy");

                    b.HasIndex("AssignedTo");

                    b.HasIndex("TaskId");

                    b.ToTable("Fieldo_WorkerTasks");
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
                    b.HasOne("Application.Models.Fieldo_UserDetails", "UserDetailsAssingedBy")
                        .WithMany()
                        .HasForeignKey("AssignedBy");

                    b.HasOne("Application.Models.Fieldo_UserDetails", "UserDetailsAssignedTo")
                        .WithMany()
                        .HasForeignKey("AssignedTo");

                    b.HasOne("Application.Models.Fieldo_UserDetails", "UserDetailsCreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Models.Fieldo_UserDetails", "UserDetailsUpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedBy");

                    b.Navigation("UserDetailsAssignedTo");

                    b.Navigation("UserDetailsAssingedBy");

                    b.Navigation("UserDetailsCreatedBy");

                    b.Navigation("UserDetailsUpdatedBy");
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

            modelBuilder.Entity("Application.Models.Fieldo_WorkerBankDetails", b =>
                {
                    b.HasOne("Application.Models.Fieldo_Bank", "Bank")
                        .WithMany()
                        .HasForeignKey("BankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Models.Fieldo_UserDetails", "UserDtailsWorker")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bank");

                    b.Navigation("UserDtailsWorker");
                });

            modelBuilder.Entity("Application.Models.Fieldo_WorkerTasks", b =>
                {
                    b.HasOne("Application.Models.Fieldo_UserDetails", "UserDetailsAssignedBy")
                        .WithMany()
                        .HasForeignKey("AssignedBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Models.Fieldo_UserDetails", "UserDetailsAssignedTo")
                        .WithMany()
                        .HasForeignKey("AssignedTo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Models.Fieldo_Task", "Task")
                        .WithMany()
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Task");

                    b.Navigation("UserDetailsAssignedBy");

                    b.Navigation("UserDetailsAssignedTo");
                });
#pragma warning restore 612, 618
        }
    }
}
