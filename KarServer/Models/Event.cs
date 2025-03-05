using KarServer.Models;
using System;
using System.Collections.Generic;
using KarServer.Data;
using KarServer.Models;


public class Event
{
    // Unique identifier for the event
    public int Id { get; set; }

    // Name of the event (e.g., "Friday Night Karaoke")
    public string Name { get; set; } = string.Empty;

    // Date and time of the event
    public DateTime Date { get; set; }

    // Foreign key linking to the host (AppUser)
    public string HostId { get; set; } = string.Empty;

    // Navigation property to the host user
    public AppUser? Host { get; set; }

    // List of song requests (queue items) for this event
    public List<QueueItem> QueueItems { get; set; } = new List<QueueItem>();
}
