using Microsoft.EntityFrameworkCore;

namespace Itan.Database
{
    internal class EntityFrameworkContext : DbContext
    {
        public EntityFrameworkContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<ChannelDownload> ChannelDownloads { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChannelDownload>()
                .HasIndex(x => new {x.ChannelId, x.HashCode})
                .IsUnique(true);
        }
    }
}