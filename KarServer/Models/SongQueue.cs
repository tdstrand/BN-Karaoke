// Models/SongQueue.cs
using KarServer.Data;
using KarServer.Models;

namespace KarServer.Models
{
    public class SongQueue
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public Song Song { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}