using Itan.Common;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<PersonPerson> PersonsPersons { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureNews(modelBuilder);
            ConfigureChannel(modelBuilder);
            ConfigureChannelsPersons(modelBuilder);
            ConfigureChannelDownload(modelBuilder);
            ConfigureChannelSubmitter(modelBuilder);
            ConfigureChannelNewsRead(modelBuilder);
            ConfigureChannelNewsOpened(modelBuilder);
            ConfigurePersonSettings(modelBuilder);
            ConfigurePersonsPersons(modelBuilder);
            ConfigureTags(modelBuilder);
            ConfigureNewsTags(modelBuilder);
        }

        private void ConfigureNewsTags(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsTag>()
                .HasKey(x => new { x.TagId, x.NewsId });

            modelBuilder.Entity<NewsTag>()
                .HasOne(x => x.News)
                .WithMany(x => x.NewsTags)
                .HasForeignKey(x => x.NewsId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NewsTag>()
                .HasOne(x => x.Tag)
                .WithMany(x => x.NewsTags)
                .HasForeignKey(x => x.TagId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureTags(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>()
                .HasIndex(x => x.Text)
                .IsUnique();

            modelBuilder.Entity<Tag>()
                .Property(x => x.Text)
                .IsRequired();
        }

        private void ConfigurePersonsPersons(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonPerson>()
                .HasOne(x => x.Target)
                .WithMany(x => x.Following)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PersonPerson>()
                .HasOne(x => x.Follower)
                .WithMany(x => x.Followed)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PersonPerson>()
                .HasIndex(x => new { x.Id, x.FollowerPersonId, x.TargetPersonId })
                .IsUnique();
        }

        private void ConfigurePersonSettings(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReaderSettings>()
                .HasOne<Person>(x => x.Person)
                .WithOne(x => x.Settings)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReaderSettings>()
                .Property(x => x.ShowUpdatedNews)
                .HasDefaultValue(UpdatedNews.Show);

            modelBuilder.Entity<ReaderSettings>()
                .Property(x => x.SquashNewsUpdates)
                .HasDefaultValue(SquashUpdate.Show);
        }

        private void ConfigureChannelNewsOpened(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChannelNewsOpened>()
                .HasOne<News>(x => x.News)
                .WithMany(x => x.ChannelNewsOpened)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChannelNewsOpened>()
                .HasOne<Channel>(x => x.Channel)
                .WithMany(x => x.ChannelNewsOpened)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChannelNewsOpened>()
                .HasOne<Person>(x => x.Person)
                .WithMany(x => x.ChannelNewsOpened)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureChannelNewsRead(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChannelNewsRead>()
                .HasIndex(x => new { x.ChannelId, x.NewsId, x.PersonId })
                .IsUnique();

            modelBuilder.Entity<ChannelNewsRead>()
                .HasOne<News>(x => x.News)
                .WithMany(x => x.ChannelNewsRead)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChannelNewsRead>()
                .HasOne<Channel>(x => x.Channel)
                .WithMany(x => x.ChannelNewsRead)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChannelNewsRead>()
                .HasOne<Person>(x => x.Person)
                .WithMany(x => x.ChannelNewsRead)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChannelNewsRead>()
                .Property(p => p.ReadType)
                .HasDefaultValue("Read");
        }

        private void ConfigureChannelSubmitter(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChannelSubmitter>()
                .HasIndex(x => x.ChannelId)
                .IsUnique();

            modelBuilder.Entity<ChannelSubmitter>()
                .HasOne<Person>(x => x.Person)
                .WithMany(x => x.SubmittedChannels)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChannelSubmitter>()
                .HasOne<Channel>(x => x.Channel)
                .WithOne(x => x.Submitter)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureChannelDownload(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChannelDownload>()
                .HasIndex(x => new { x.ChannelId, SHA256 = x.SHA256 })
                .IsUnique(true);
        }

        private void ConfigureChannelsPersons(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChannelsPersons>()
                .HasOne<Channel>(x => x.Channel)
                .WithMany(x => x.PersonSubscribers)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChannelsPersons>()
                .HasOne<Person>(x => x.Reader)
                .WithMany(x => x.SubscribedChannels)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureChannel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Channel>()
                .HasOne<ChannelSubmitter>(x => x.Submitter)
                .WithOne(x => x.Channel)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureNews(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<News>()
                .HasIndex(x => new { x.ChannelId, SHA256 = x.Sha256 })
                .IsUnique(true);

            modelBuilder.Entity<News>()
                .HasOne<Channel>(x => x.Channel)
                .WithMany(x => x.News)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}