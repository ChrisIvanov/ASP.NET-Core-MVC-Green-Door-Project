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
        [Range(AlbumPriceMinValue, AlbumPriceMaxValue)]
        public decimal Price { get; set; }

        public string RatingId { get; set; }
        [Range(0, 5)]
        public Rating Rating { get; set; }

        public IEnumerable<Song> Songs { get; set; } = new List<Song>();
    }
}
