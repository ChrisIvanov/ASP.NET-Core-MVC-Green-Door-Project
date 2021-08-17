namespace GreenDoorProject.Models.Music
{
    using System.Collections.Generic;

    public class AlbumDetailsViewModel
    {
        public string Id { get; set; }

        public string Artist { get; set; }

        public string AlbumTitle { get; set; }

        public string ImagePath { get; set; }

        public double Rating { get; set; }

        public IEnumerable<SongFormModel> Songs { get; set; } = new List<SongFormModel>();
    }
}
