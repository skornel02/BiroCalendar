﻿// <auto-generated />
using System;
using BiroCalendar.Api.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BiroCalendar.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240120212047_BasicDataStructure")]
    partial class BasicDataStructure
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("BiroCalendar.Api.Persistance.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EmailAddress")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("BiroCalendar.Api.Persistance.Entities.BiroAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccountId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("AccountPassword")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastAccessed")
                        .HasColumnType("TEXT");

                    b.Property<string>("ServiceUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("BiroAccounts");
                });

            modelBuilder.Entity("BiroCalendar.Api.Persistance.Entities.BiroRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BiroAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("FetchedAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Outdated")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("OutdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TaskDueDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("TaskName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BiroAccountId");

                    b.ToTable("Records");
                });

            modelBuilder.Entity("BiroCalendar.Api.Persistance.Entities.BiroAccount", b =>
                {
                    b.HasOne("BiroCalendar.Api.Persistance.Entities.Account", "Account")
                        .WithMany("BiroAccounts")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("BiroCalendar.Api.Persistance.Entities.BiroRecord", b =>
                {
                    b.HasOne("BiroCalendar.Api.Persistance.Entities.BiroAccount", "BiroAccount")
                        .WithMany("BiroRecords")
                        .HasForeignKey("BiroAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BiroAccount");
                });

            modelBuilder.Entity("BiroCalendar.Api.Persistance.Entities.Account", b =>
                {
                    b.Navigation("BiroAccounts");
                });

            modelBuilder.Entity("BiroCalendar.Api.Persistance.Entities.BiroAccount", b =>
                {
                    b.Navigation("BiroRecords");
                });
#pragma warning restore 612, 618
        }
    }
}