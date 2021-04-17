﻿// <auto-generated />
using System;
using Itan.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Itan.Database.Migrations
{
    [DbContext(typeof(EntityFrameworkContext))]
    partial class EntityFrameworkContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Itan.Database.Channel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Channels");
                });

            modelBuilder.Entity("Itan.Database.ChannelDownload", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChannelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SHA256")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("ChannelId", "SHA256")
                        .IsUnique();

                    b.ToTable("ChannelDownloads");
                });

            modelBuilder.Entity("Itan.Database.ChannelNewsOpened", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChannelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("NewsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.HasIndex("NewsId");

                    b.HasIndex("PersonId");

                    b.ToTable("ChannelNewsOpened");
                });

            modelBuilder.Entity("Itan.Database.ChannelNewsRead", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChannelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("NewsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ReadType")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("Read");

                    b.HasKey("Id");

                    b.HasIndex("NewsId");

                    b.HasIndex("PersonId");

                    b.HasIndex("ChannelId", "NewsId", "PersonId")
                        .IsUnique();

                    b.ToTable("ChannelNewsReads");
                });

            modelBuilder.Entity("Itan.Database.ChannelSubmitter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChannelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId")
                        .IsUnique();

                    b.HasIndex("PersonId");

                    b.ToTable("ChannelsSubmitters");
                });

            modelBuilder.Entity("Itan.Database.ChannelsPersons", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChannelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.HasIndex("PersonId");

                    b.ToTable("ChannelsPersons");
                });

            modelBuilder.Entity("Itan.Database.News", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChannelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("OriginalPostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Published")
                        .HasColumnType("datetime2");

                    b.Property<string>("SHA256")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId", "SHA256")
                        .IsUnique();

                    b.ToTable("News");
                });

            modelBuilder.Entity("Itan.Database.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("Itan.Database.PersonPerson", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("FollowerPersonId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TargetPersonId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("FollowerPersonId");

                    b.HasIndex("TargetPersonId");

                    b.HasIndex("Id", "FollowerPersonId", "TargetPersonId")
                        .IsUnique();

                    b.ToTable("PersonsPersons");
                });

            modelBuilder.Entity("Itan.Database.ReaderSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ShowUpdatedNews")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(25)")
                        .HasMaxLength(25)
                        .HasDefaultValue("Show");

                    b.Property<string>("SquashNewsUpdates")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(25)")
                        .HasMaxLength(25)
                        .HasDefaultValue("Show");

                    b.HasKey("Id");

                    b.HasIndex("PersonId")
                        .IsUnique();

                    b.ToTable("ReadersSettings");
                });

            modelBuilder.Entity("Itan.Database.ChannelDownload", b =>
                {
                    b.HasOne("Itan.Database.Channel", "Channel")
                        .WithMany("Downloads")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Itan.Database.ChannelNewsOpened", b =>
                {
                    b.HasOne("Itan.Database.Channel", "Channel")
                        .WithMany("ChannelNewsOpened")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Itan.Database.News", "News")
                        .WithMany("ChannelNewsOpened")
                        .HasForeignKey("NewsId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Itan.Database.Person", "Person")
                        .WithMany("ChannelNewsOpened")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Itan.Database.ChannelNewsRead", b =>
                {
                    b.HasOne("Itan.Database.Channel", "Channel")
                        .WithMany("ChannelNewsRead")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Itan.Database.News", "News")
                        .WithMany("ChannelNewsRead")
                        .HasForeignKey("NewsId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Itan.Database.Person", "Person")
                        .WithMany("ChannelNewsRead")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Itan.Database.ChannelSubmitter", b =>
                {
                    b.HasOne("Itan.Database.Channel", "Channel")
                        .WithOne("Submitter")
                        .HasForeignKey("Itan.Database.ChannelSubmitter", "ChannelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Itan.Database.Person", "Person")
                        .WithMany("SubmittedChannels")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Itan.Database.ChannelsPersons", b =>
                {
                    b.HasOne("Itan.Database.Channel", "Channel")
                        .WithMany("PersonSubscribers")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Itan.Database.Person", "Reader")
                        .WithMany("SubscribedChannels")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Itan.Database.News", b =>
                {
                    b.HasOne("Itan.Database.Channel", "Channel")
                        .WithMany("News")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Itan.Database.PersonPerson", b =>
                {
                    b.HasOne("Itan.Database.Person", "Follower")
                        .WithMany("Followed")
                        .HasForeignKey("FollowerPersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Itan.Database.Person", "Target")
                        .WithMany("Following")
                        .HasForeignKey("TargetPersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Itan.Database.ReaderSettings", b =>
                {
                    b.HasOne("Itan.Database.Person", "Person")
                        .WithOne("Settings")
                        .HasForeignKey("Itan.Database.ReaderSettings", "PersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
