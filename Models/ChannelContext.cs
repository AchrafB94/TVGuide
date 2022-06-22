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
        }

        public DbSet<Channel> Channels { get; set; }
    }
}
