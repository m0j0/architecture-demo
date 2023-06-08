﻿// <auto-generated />
using System;
using ArchitectureDemo.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ArchitectureDemo.DAL.Migrations
{
    [DbContext(typeof(DemoContext))]
    [Migration("20230226184536_OnCascade")]
    partial class OnCascade
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ArchitectureDemo.DAL.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uuid")
                        .HasColumnName("parent_id");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("users_email_key");

                    b.HasIndex("ParentId")
                        .HasDatabaseName("ix_users_parent_id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("ArchitectureDemo.DAL.Entities.UserFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_files");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_files_user_id");

                    b.ToTable("user_files", (string)null);
                });

            modelBuilder.Entity("ArchitectureDemo.DAL.Entities.User", b =>
                {
                    b.HasOne("ArchitectureDemo.DAL.Entities.User", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("users_parent_id_fkey");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("ArchitectureDemo.DAL.Entities.UserFile", b =>
                {
                    b.HasOne("ArchitectureDemo.DAL.Entities.User", "User")
                        .WithMany("Files")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("fk_user_files_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ArchitectureDemo.DAL.Entities.User", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("Files");
                });
#pragma warning restore 612, 618
        }
    }
}
