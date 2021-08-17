namespace GreenDoorProject.Controllers
{
    using System.Linq;
    using GreenDoorProject.Data;
    using GreenDoorProject.Models.Music;
    using GreenDoorProject.Services.Music;
    using Microsoft.AspNetCore.Mvc;

    public class MusicController : Controller
    {
        private readonly GreenDoorProjectDbContext data;
        private readonly IMusicService music;

        public MusicController(GreenDoorProjectDbContext data, IMusicService music)
        {
            this.data = data;
            this.music = music;
        }

        public IActionResult All([FromQuery] AllMusicQueryModel query)
        {
            if (!data.MusicAlbums.Any())
            {
                var error = "Currently there are no music albums in the music library.";

                var errorModel = new AllMusicQueryModel
                {
                    ModelError = error
                };

                return View(errorModel);
            }

            var queryResult = this.music.All(
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllMusicQueryModel.AlbumsPerPage,
                query.ShowOnlySongs);

            var totalResults = queryResult.TotalResults;

            query.TotalResults = totalResults;
            query.ShowOnlySongs = queryResult.ShowOnlySongs;
            query.Albums = queryResult.Albums;
            query.Songs = queryResult.Songs;

            return View(query);
        }
    }
}
