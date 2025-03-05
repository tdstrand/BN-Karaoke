// Data/AppDbContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KarServer.Models;
using Microsoft.AspNetCore.Identity;


namespace KarServer.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Song> Songs { get; set; }
        public DbSet<SongQueue> SongQueue { get; set; }
        public DbSet<FavoriteSong> FavoriteSongs { get; set; }
    }
}