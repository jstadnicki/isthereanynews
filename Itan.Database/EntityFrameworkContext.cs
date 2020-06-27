using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Itan.Database
{
    internal class EntityFrameworkContext : DbContext
    {
        public EntityFrameworkContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Channel> Channels { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<ChannelDownload> ChannelDownloads { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<ChannelsPersons> ChannelsPersons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChannelDownload>()
                .HasIndex(x => new {x.ChannelId, x.SHA256})
                .IsUnique(true);
            
            modelBuilder.Entity<News>()
                .HasIndex(x => new {x.ChannelId, x.SHA256})
                .IsUnique(true);
            
            modelBuilder.Entity<News>()
                .HasOne<Channel>(x => x.Channel)
                .WithMany(x => x.News)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChannelsPersons>()
                .HasOne<Channel>(x => x.Channel)
                .WithMany(x => x.PersonSubscribers)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChannelsPersons>()
                .HasOne<Person>(x => x.Reader)
                .WithMany(x => x.SubscribedChannels)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Channel>()
                .HasOne<ChannelSubmitter>(x => x.Submitter)
                .WithOne(x => x.Channel)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Person>()
                .HasMany<ChannelSubmitter>(x => x.SubmittedChannels)
                .WithOne(x => x.Submitter)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChannelSubmitter>()
                .HasIndex(x => x.ChannelId)
                .IsUnique();
        }
    }
}