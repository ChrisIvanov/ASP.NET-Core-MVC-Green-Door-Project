namespace GreenDoorProject.Services.Music.Models
{ 
    using System;
    
    public class SongsDetailsViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public TimeSpan SongDuration { get; set; }

        public string AlbumId { get; set; }
    }
}
