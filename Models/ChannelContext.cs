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
            // connect to mysql with connection string from app settings
            var connectionString = Configuration.GetConnectionString("Database");
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        DbSet<Channel> Channels { get; set; }
    }
}
