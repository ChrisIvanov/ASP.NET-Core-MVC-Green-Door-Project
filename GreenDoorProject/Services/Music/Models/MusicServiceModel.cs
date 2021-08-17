namespace GreenDoorProject.Services.Music.Models
{
    using System.Collections.Generic;
    
    public class MusicServiceModel
    {
        public string Id { get; set; }

        public string Artist { get; set; }

        public string AlbumTitle { get; set; }

        public string ImagePath { get; set; }

        public double Rating { get; set; }

        public IEnumerable<SongsDetailsViewModel> Songs { get; set; } = new List<SongsDetailsViewModel>();
    }
}
