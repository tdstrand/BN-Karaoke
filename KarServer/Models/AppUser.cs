// Models/AppUser.cs
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using KarServer.Data;
using KarServer.Models;

namespace KarServer.Models
{
    public class AppUser : IdentityUser
    {
        public string? FavoriteGenre { get; set; }
        public ICollection<FavoriteSong>? FavoriteSongs { get; set; }
    }

    public class FavoriteSong
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
