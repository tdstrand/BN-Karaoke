using KarServer.Models;
using KarServer.Data;

public class QueueItem
{
    // Unique identifier for the queue item
    public int Id { get; set; }

    // Foreign key linking to the event this queue item belongs to
    public int EventId { get; set; }

    // Navigation property to the event
    public Event? Event { get; set; }

    // Foreign key linking to the singer (AppUser)
    public string UserId { get; set; } = string.Empty;

    // Navigation property to the singer
    public AppUser? User { get; set; }

    // Foreign key linking to the song being requested
    public int QueueSongId { get; set; }

    // Navigation property to the song
    public Song? Song { get; set; }

    // Position of this item in the queue (e.g., 1 = next to sing)
    public int QueuePosition { get; set; }
}