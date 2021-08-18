namespace GreenDoorProject.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GreenDoorProject.Data;
    using GreenDoorProject.Data.Models;
    using GreenDoorProject.Infrastructure;
    using GreenDoorProject.Models.Music;
    using GreenDoorProject.Services.Music;
    using GreenDoorProject.Services.Music.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static AdminConstants;

    [Area(AdminAreaName)]
    [Authorize(Roles = AdministratorRoleName)]
    public class MusicController : Controller
    {
        private readonly IMusicService music;
        private readonly GreenDoorProjectDbContext data;

        public MusicController(IMusicService music, GreenDoorProjectDbContext data)
        {
            this.music = music;
            this.data = data;
        }

        public IActionResult AdminAll([FromQuery] AllMusicQueryModel query)
        {
            if (!data.MusicAlbums.Any())
            {
                var error = "Currently there is no music in the music library.";

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

            query.Albums = queryResult.Albums;
            query.TotalResults = totalResults;
            query.ShowOnlySongs = queryResult.ShowOnlySongs;
            query.Albums = queryResult.Albums;
            query.Songs = queryResult.Songs;

            return View(query);
        }

        [HttpGet]
        [Authorize]
        public IActionResult AdminAdd()
        {
            return View(new AlbumFormModel());
        }

        [HttpPost]
        [Authorize]
        public IActionResult AdminAdd(AlbumFormModel albumModel)
        {
            if (!ModelState.IsValid)
            {
                return View(albumModel);
            }

            if (ExistingMusicAlbum(albumModel))
            {
                this.ModelState
                    .AddModelError(
                        nameof(albumModel.AlbumTitle),
                        "The music album already exists.");
            }

            var album = new MusicAlbum
            {
                AlbumTitle = albumModel.AlbumTitle,
                Artist = albumModel.Artist,
                ImagePath = albumModel.ImagePath,
                Rating = albumModel.Rating

            };

            this.data.MusicAlbums.Add(album);
            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }


        [Authorize]
        public IActionResult AddSong(string albumId)
            => View(new SongFormModel());

        [HttpPost]
        [Authorize]
        public IActionResult AddSong(SongFormModel model, string albumId)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (ExistingSongInAlbum(model, albumId))
            {
                this.ModelState
                    .AddModelError(
                        nameof(model.Name),
                        "This song is already in the album.");
            }

            var album = this.data.MusicAlbums.Find(albumId);

            var song = new Song();

            song.Name = model.Name;
            song.SongDuration = model.SongDuration;
            song.MusicAlbumId = model.AlbumId;

            album.Songs.Add(song);

            this.data.Songs.Add(song);

            return RedirectToAction("AlbumDetails", "Music");
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(AlbumFormModel model, string id)
        {
            if (!User.IsAdmin())
            {
                ModelState.AddModelError
                    (string.Empty, "You are not authorized to make changes to the website content.");

                return RedirectToAction("AdminAll", "Books");
            }

            var album = this.data.MusicAlbums
                .Where(b => b.Id == id)
                .FirstOrDefault();

            var songList = new List<SongsDetailsViewModel>();

            foreach (var song in album.Songs)
            {
                var modelSong = new SongsDetailsViewModel
                {
                    Name = song.Name,
                    SongDuration = song.SongDuration,
                    AlbumId = song.MusicAlbumId
                };

                songList.Add(modelSong);
            }

            var edited = this.music.Edit(
                id,
                album.AlbumTitle,
                album.Artist,
                album.ImagePath,
                album.Rating,
                songList);

            if (!edited)
            {
                return BadRequest();
            }

            return RedirectToAction("AdminAll", "Books");
        }

        [Authorize]
        public IActionResult EditSong(
                string id,
                string name,
                TimeSpan duration)
        {
            var song = this.data.Songs
                .Where(s => s.Id == id)
                .FirstOrDefault();

            var edited = music.EditSong(
                song.Id,
                song.Name,
                song.SongDuration);

            if (!edited)
            {
                return BadRequest();
            }

            return RedirectToAction("AdminAll", "Music");
        }

        [Authorize]
        public IActionResult Delete(string id)
        {
            var album = this.data.MusicAlbums.Find(id);

            if (album == null)
            {
                return BadRequest();
            }

            this.data.MusicAlbums.Remove(album);
            this.data.SaveChanges();

            return RedirectToAction("AdminAll", "Music");
        }

        [Authorize]
        public IActionResult DeleteSong(string id)
        {
            var song = this.data.Songs.Find(id);

            if (song == null)
            {
                return BadRequest();
            }

            this.data.Songs.Remove(song);
            this.data.SaveChanges();

            return RedirectToAction("AdminAll", "Music");
        }

        public IActionResult AlbumDetails(string id)
        {
            var album = this.data.MusicAlbums.Find(id);

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

        private bool ExistingMusicAlbum(AlbumFormModel albumModel)
            => this.data.MusicAlbums
                .Any(a => a.AlbumTitle == albumModel.AlbumTitle);

        private bool ExistingSongInAlbum(SongFormModel model, string albumId)
            => this.data.Songs
            .Any(s => s.Name == model.Name && s.MusicAlbumId == model.AlbumId);

    }
}
