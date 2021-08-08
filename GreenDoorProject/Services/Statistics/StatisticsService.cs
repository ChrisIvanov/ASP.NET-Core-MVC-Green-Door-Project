namespace GreenDoorProject.Services.Statistics
{
    using GreenDoorProject.Data;
    using System.Linq;

    public class StatisticsService : IStatisticsService
    {
        private readonly GreenDoorProjectDbContext _data;

        public StatisticsService(GreenDoorProjectDbContext data) 
            => _data = data;

        public StatisticsServiceModel Total()
        {
            var totalBooks = this._data.Books.Count();
            var totalMovies = this._data.Movies.Count();
            var totalMusicAlbums = this._data.MusicAlbums.Count();
            var totalUsers = this._data.Users.Count();
            var totalMembers = this._data.Members.Count();
            var totalPatrons = this._data.Patrons.Count();

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
