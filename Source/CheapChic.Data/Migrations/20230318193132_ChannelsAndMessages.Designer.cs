﻿// <auto-generated />
using System;
using CheapChic.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CheapChic.Data.Migrations
{
    [DbContext(typeof(CheapChicContext))]
    [Migration("20230318193132_ChannelsAndMessages")]
    partial class ChannelsAndMessages
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CheapChic.Data.Entities.TelegramBotEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ChangedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Disabled")
                        .HasColumnType("boolean");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.ToTable("TelegramBot", "CheapChic");
                });

            modelBuilder.Entity("CheapChic.Data.Entities.TelegramChannelEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ChangedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ChatId")
                        .IsUnique();

                    b.HasIndex("OwnerId");

                    b.ToTable("TelegramChannel", "CheapChic");
                });

            modelBuilder.Entity("CheapChic.Data.Entities.TelegramMessageEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ChangedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ChannelId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("MessageId")
                        .HasColumnType("integer");

                    b.Property<Guid?>("TelegramBotId")
                        .HasColumnType("uuid");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.HasIndex("TelegramBotId");

                    b.HasIndex("UserId");

                    b.HasIndex("UserId", "ChannelId", "MessageId")
                        .IsUnique();

                    b.ToTable("TelegramMessage", "CheapChic");
                });

            modelBuilder.Entity("CheapChic.Data.Entities.TelegramUserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ChangedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Disabled")
                        .HasColumnType("boolean");

                    b.Property<string>("Lastname")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ChatId")
                        .IsUnique();

                    b.ToTable("TelegramUser", "CheapChic");
                });

            modelBuilder.Entity("CheapChic.Data.Entities.TelegramBotEntity", b =>
                {
                    b.HasOne("CheapChic.Data.Entities.TelegramUserEntity", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("CheapChic.Data.Entities.TelegramChannelEntity", b =>
                {
                    b.HasOne("CheapChic.Data.Entities.TelegramUserEntity", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("CheapChic.Data.Entities.TelegramMessageEntity", b =>
                {
                    b.HasOne("CheapChic.Data.Entities.TelegramChannelEntity", "Channel")
                        .WithMany()
                        .HasForeignKey("ChannelId");

                    b.HasOne("CheapChic.Data.Entities.TelegramBotEntity", "TelegramBot")
                        .WithMany()
                        .HasForeignKey("TelegramBotId");

                    b.HasOne("CheapChic.Data.Entities.TelegramUserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Channel");

                    b.Navigation("TelegramBot");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
