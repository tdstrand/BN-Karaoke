// Models/Song.cs

using KarServer.Data;
using KarServer.Models;

namespace KarServer.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
    }
}