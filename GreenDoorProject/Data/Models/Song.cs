namespace GreenDoorProject.Data.Models
{
    using System;
    
    public class Song
    {
        public string Id { get; init; }

        public string Name { get; set; }

        public TimeSpan SongDuration { get; set; }

        public string AlbumId { get; set; }
        public Music Album { get; set; }
    }
}