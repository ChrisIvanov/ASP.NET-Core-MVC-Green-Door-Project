namespace GreenDoorProject.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Song
    {
        public string Id { get; init; }

        [Required]
        public string Name { get; set; }

        [Required]
        public TimeSpan SongDuration { get; set; }

        public string AlbumId { get; set; }
        public Music Album { get; set; }
    }
}