﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using massager_laba.Data;

#nullable disable

namespace massagerlaba.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20240619120825_addType")]
    partial class addType
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("massager_laba.Data.Model.MessageInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("FromUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ToUserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("MessageInfos");
                });

            modelBuilder.Entity("massager_laba.Data.Model.MessagerModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("IdUserFrom")
                        .HasColumnType("uuid");

                    b.Property<Guid>("IdUserWhere")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsCheked")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastLetter")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("TypeMessage")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("MessagerModels");
                });

            modelBuilder.Entity("massager_laba.Data.Model.UserModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AvatarUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
