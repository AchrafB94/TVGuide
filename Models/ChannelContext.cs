using Microsoft.EntityFrameworkCore;

namespace TVGuide.Models
{
    public class ChannelContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public ChannelContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server with connection string from app settings
            options.UseSqlServer(Configuration.GetConnectionString("Database"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<Channel>().HasIndex(c => c.Position).IsUnique();
            modelBuilder.Entity<Channel>().HasIndex(c => c.XML);

        }

        public DbSet<Channel> Channels { get; set; }
    }
}
