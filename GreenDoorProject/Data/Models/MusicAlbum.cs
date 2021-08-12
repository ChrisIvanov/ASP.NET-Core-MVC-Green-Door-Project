namespace GreenDoorProject.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Music;

    public class MusicAlbum
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(ArtistNameMaxLength, MinimumLength = ArtistNameMinLength)]
        public string Artist { get; set; }

        [Required]
        [StringLength(AlbumTitleMaxLength, MinimumLength = AlbumTitleMinLength)]
        public string AlbumTitle { get; set; }

        [Required]
        [Range(0, 10)]
        public double Rating { get; set; }

        public IEnumerable<Song> Songs { get; set; } = new List<Song>();
    }
}
