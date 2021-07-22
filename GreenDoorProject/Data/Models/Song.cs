namespace GreenDoorProject.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Song
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }

        [Required]
        public TimeSpan SongDuration { get; set; }

        [Required]
        public string AlbumId { get; set; }
        public Music Album { get; set; }

        [Required]
        public string AdminId { get; init; }
        public Admin Admin { get; init; }
    }
}