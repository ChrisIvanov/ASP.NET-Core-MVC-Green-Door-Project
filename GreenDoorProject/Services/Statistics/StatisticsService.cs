namespace GreenDoorProject.Services.Statistics
{
    using System.Linq;
    using GreenDoorProject.Data;
    using GreenDoorProject.Services.Statistics.Models;

    public class StatisticsService : IStatisticsService
    {
        private readonly GreenDoorProjectDbContext data;

        public StatisticsService(GreenDoorProjectDbContext data) 
            => this.data = data;

        public StatisticsServiceModel Total()
        {
            var totalBooks = this.data.Books.Count();
            var totalMovies = this.data.Movies.Count();
            var totalMusicAlbums = this.data.MusicAlbums.Count();
            var totalGuests = this.data.Genres.Count();
            var totalMembers = this.data.Members.Count();
            var totalPatrons = this.data.Patrons.Count();

            return new StatisticsServiceModel
            {
                TotalBooks = totalBooks,
                TotalMovies = totalMovies,
                TotalMusicAlbums = totalMusicAlbums,
                TotalGuests = totalGuests,
                TotalMembers = totalMembers,
                TotalPatrons = totalPatrons
            };
        }
    }
}
