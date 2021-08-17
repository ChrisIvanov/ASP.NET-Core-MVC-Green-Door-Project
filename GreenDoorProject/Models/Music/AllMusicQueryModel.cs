namespace GreenDoorProject.Models.Music
{
    using GreenDoorProject.Services.Music.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AllMusicQueryModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }

        public MusicSorting Sorting { get; set; }

        public bool ShowOnlySongs { get; set; }

        public int CurrentPage { get; set; }

        public const int AlbumsPerPage = 20;

        public string ModelError { get; set; }

        public int TotalResults { get; set; }

        public IEnumerable<MusicServiceModel> Albums { get; set; }

        public IEnumerable<SongsDetailsViewModel> Songs { get; set; }
    }
}
