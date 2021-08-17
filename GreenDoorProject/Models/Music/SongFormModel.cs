namespace GreenDoorProject.Models.Music
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Music;

    public class SongFormModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(SongNameMaxLength, MinimumLength = SongNameMinLength)]
        public string Name { get; set; }

        [Required]
        public TimeSpan SongDuration { get; set; }

        public string AlbumId { get; set; }
    }
}
