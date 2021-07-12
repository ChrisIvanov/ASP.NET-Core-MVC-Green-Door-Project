namespace GreenDoorProject.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Music
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string Artist { get; set; }

        [Required]
        public string AlbumTitle { get; set; }

        [Required]
        public decimal Price { get; set; }

        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
