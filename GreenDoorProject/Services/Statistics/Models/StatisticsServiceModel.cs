namespace GreenDoorProject.Services.Statistics.Models
{
    using GreenDoorProject.Models.Api.Books;
    
    public class StatisticsServiceModel
    {
        public int TotalBooks { get; set; }

        public int TotalMovies { get; set; }

        public int TotalMusicAlbums { get; set; }

        public int TotalGuests { get; set; }

        public int TotalMembers { get; set; }

        public int TotalPatrons { get; set; }
    }
}
