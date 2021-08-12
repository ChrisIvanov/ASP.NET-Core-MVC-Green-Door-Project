namespace GreenDoorProject.Services.Statistics
{
    using GreenDoorProject.Data;
    using System.Linq;

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
            var totalUsers = this.data.Users.Count();
            var totalMembers = this.data.Members.Count();
            var totalPatrons = this.data.Patrons.Count();

            return new StatisticsServiceModel
            {
                TotalBooks = totalBooks,
                TotalMovies = totalMovies,
                TotalMusicAlbums = totalMusicAlbums,
                TotalUsers = totalUsers,
                TotalMembers = totalMembers,
                TotalPatrons = totalPatrons
            };
        }
    }
}
