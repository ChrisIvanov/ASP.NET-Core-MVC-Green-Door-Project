namespace GreenDoorProject.Services.Music.Models
{
    using System.Collections.Generic;

    public class MusicQueryServiceModel
    {
        public int AlbumsPerPage { get; init; }

        public int CurrentPage { get; init; }

        public int TotalResults { get; init; }

        public string ModelError { get; init; }

        public bool ShowOnlySongs { get; init; }

        public IEnumerable<MusicServiceModel> Albums { get; init; } = new List<MusicServiceModel>();

        public IEnumerable<SongsDetailsViewModel> Songs { get; init; } = new List<SongsDetailsViewModel>();
    }
}
