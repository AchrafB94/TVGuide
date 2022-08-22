using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TVGuide.Models
{
    public class ChannelContext : IdentityDbContext<TVGuideUser>
    {
        protected readonly IConfiguration Configuration;

        public ChannelContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server with connection string from app settings
            options.UseSqlite(Configuration.GetConnectionString("Database"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(new List<Category> {
                new Category { Id = 1, Name = "Movies" },
                new Category { Id = 2, Name = "Series" },
                new Category { Id = 3, Name = "Sports" },
                new Category { Id = 4, Name = "Documentary" },
                new Category { Id = 5, Name = "News" },
                new Category { Id = 6, Name = "Kids" },
                new Category { Id = 7, Name = "Music" }
            });

            modelBuilder.Entity<Package>().HasData(new List<Package> {
                new Package { Id = 1, Name = "OSN" },
                new Package { Id = 2, Name = "Canal" },
                new Package { Id = 3, Name = "beIN" }
            });

            modelBuilder.Entity<TVGuideUser>().HasMany(u => u.favoriteChannels).WithOne(fc => fc.User);
        }

        public DbSet<Channel> Channels { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<TVGuideUser> Users { get; set; }
        public DbSet<FavoriteChannel> FavoriteChannels { get; set; }
    }
}
