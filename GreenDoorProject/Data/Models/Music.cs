namespace GreenDoorProject.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Music;

    public class Music
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

        [Required]
        public string AdminId { get; init; }
        public Admin Admin { get; init; }

        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
