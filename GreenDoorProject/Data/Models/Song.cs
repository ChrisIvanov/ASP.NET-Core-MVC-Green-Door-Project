namespace GreenDoorProject.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Song;

    public class Song
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(SongNameMaxLength, MinimumLength = SongNameMinLength)]
        public string Name { get; set; }

        [Required]
        public TimeSpan SongDuration { get; set; }

        [Required]
        public string MusicAlbumId { get; set; }
        public MusicAlbum MusicAlbum { get; set; }

        [Required]
        public string PatronId { get; init; }
        public Patron Patron { get; init; }
    }
}