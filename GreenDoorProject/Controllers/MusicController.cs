namespace GreenDoorProject.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using GreenDoorProject.Data;
    using GreenDoorProject.Models.Music;
    using GreenDoorProject.Services.Music;
    using GreenDoorProject.Services.Music.Models;
    using Microsoft.AspNetCore.Authorization;
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


        [Authorize]
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

        [HttpGet]
        public IActionResult Details([FromRoute] string albumId)
        {
            var album = this.data.MusicAlbums.Find(albumId);

            var modelSongs = new List<SongsDetailsViewModel>();

            if (album.Songs != null)
            {
                foreach (var song in album.Songs)
                {
                    var modelSong = new SongsDetailsViewModel
                    {
                        Name = song.Name,
                        SongDuration = song.SongDuration,
                        AlbumId = song.MusicAlbumId
                    };

                    modelSongs.Add(modelSong);
                }
            }
            var albumDetails = new MusicServiceModel
            {
                Id = album.Id,
                AlbumTitle = album.AlbumTitle,
                Artist = album.Artist,
                ImagePath = album.ImagePath,
                Rating = album.Rating,
                Songs = modelSongs
            };

            return View(albumDetails);
        }

        [Authorize]
        public IActionResult PlayMusic()
            => View();
    }
}
