namespace GreenDoorProject.Models.Music
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Music;

    public class AlbumFormModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(ArtistNameMaxLength, MinimumLength = ArtistNameMinLength)]
        public string Artist { get; set; }

        [Required]
        [StringLength(AlbumTitleMaxLength, MinimumLength = AlbumTitleMinLength)]
        public string AlbumTitle { get; set; }

        [Required]
        [Url]
        public string ImagePath { get; set; }

        [Required]
        [Range(0, 10)]
        public double Rating { get; set; }

        public IEnumerable<SongFormModel> Songs { get; set; } = new List<SongFormModel>();
    }
}
