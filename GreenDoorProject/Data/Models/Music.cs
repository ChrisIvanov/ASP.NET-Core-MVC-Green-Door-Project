namespace GreenDoorProject.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Music
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public string Artist { get; set; }

        public string AlbumTitle { get; set; }

        public decimal Price { get; set; }

        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
