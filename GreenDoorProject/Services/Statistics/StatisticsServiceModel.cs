using GreenDoorProject.Models.Api.Books;

namespace GreenDoorProject.Services.Statistics
{
    public class StatisticsServiceModel
    {
        public int TotalBooks { get; set; }

        public int TotalMovies { get; set; }

        public int TotalMusicAlbums { get; set; }

        public int TotalGames { get; set; }

        public int TotalUsers { get; set; }

        public int TotalMembers { get; set; }

        public int TotalPatrons { get; set; }
    }
}
