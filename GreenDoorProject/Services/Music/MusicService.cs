using GreenDoorProject.Data;
using GreenDoorProject.Data.Models;
using GreenDoorProject.Models.Music;
using GreenDoorProject.Services.Music.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GreenDoorProject.Services.Music
{
    public class MusicService : IMusicService
    {
        private readonly GreenDoorProjectDbContext data;

        public MusicService(GreenDoorProjectDbContext data)
        {
            this.data = data;
        }

        public MusicQueryServiceModel All(
            string searchTerm,
            MusicSorting sorting,
            int currentPage,
            int albumsPerPage,
            bool showOnlySongs)
        {
            var musicQuery = this.data.MusicAlbums.AsQueryable();


            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                musicQuery = musicQuery.Where(m =>
                    m.AlbumTitle.ToLower().Contains(searchTerm.ToLower())
                    || m.Artist.Contains(searchTerm.ToLower())
                    || m.Songs.Select(s => s.Name).Contains(searchTerm.ToLower()));
            }

            var totalResults = musicQuery.Count();

            var songs = this.data.MusicAlbums.Select(a => a.Songs).FirstOrDefault();
            var songList = new List<SongsDetailsViewModel>();

            if (showOnlySongs)
            {
                foreach (var song in songs)
                {
                    songList.Add(new SongsDetailsViewModel
                    {
                        Name = song.Name,
                        SongDuration = song.SongDuration
                    });
                }
            }

            var albums = musicQuery
                .Skip(((currentPage <= 1 ? 1 : currentPage) - 1) * albumsPerPage)
                .Take(albumsPerPage)
                .OrderBy(a => a.Id)
                .Select(b => new MusicServiceModel
                {
                    Id = b.Id,
                    AlbumTitle = b.AlbumTitle,
                    Artist = b.Artist,
                    ImagePath = b.ImagePath,
                    Rating = b.Rating,
                    Songs = songList
                })
                .ToList();

            musicQuery = sorting switch
            {
                MusicSorting.AlbumTitleAscending => musicQuery.OrderBy(m => m.AlbumTitle),
                MusicSorting.AlbumTitleDescending => musicQuery.OrderByDescending(m => m.AlbumTitle),
                MusicSorting.SongNameAscending => musicQuery.OrderBy(m => m.Artist),
                MusicSorting.SongNameDescending => musicQuery.OrderByDescending(m => m.Artist),
                MusicSorting.RatingAscending => musicQuery.OrderByDescending(b => b.Rating),
                MusicSorting.RatingDescending => musicQuery.OrderByDescending(b => b.Rating),
                _ => musicQuery.OrderByDescending(b => b.Id)
            };

            return new MusicQueryServiceModel
            {
                CurrentPage = currentPage,
                AlbumsPerPage = albumsPerPage,
                ShowOnlySongs = showOnlySongs,
                TotalResults = totalResults,
                Albums = albums,
                Songs = songList
            };
        }
        //public MusicServiceModel Details(string id)
        //{
        //    var album = this.data.MusicAlbums.Find(id);

        //    var modelSongs = new List<SongsDetailsViewModel>();

        //    if (album.Songs != null)
        //    {
        //        foreach (var song in album.Songs)
        //        {
        //            var modelSong = new SongsDetailsViewModel
        //            {
        //                Name = song.Name,
        //                SongDuration = song.SongDuration,
        //                AlbumId = song.MusicAlbumId
        //            };

        //            modelSongs.Add(modelSong);
        //        }
        //    }
        //    return new MusicServiceModel
        //    {
        //        Id = album.Id,
        //        AlbumTitle = album.AlbumTitle,
        //        Artist = album.Artist,
        //        ImagePath = album.ImagePath,
        //        Rating = album.Rating,
        //        Songs = modelSongs
        //    };
        //}

        public bool Edit(string id,
           string albumTitle,
           string artist,
           string imagePath,
           double rating,
           IEnumerable<SongsDetailsViewModel> modelSongs)
        {
            var albumData = this.data.MusicAlbums.Find(id);

            if (albumData == null)
            {
                return false;
            }

            var songs = new List<Song>();

            foreach (var modelSong in modelSongs)
            {
                var song = new Song();

                song.Name = modelSong.Name;
                song.SongDuration = modelSong.SongDuration;
                song.MusicAlbumId = modelSong.AlbumId;

                songs.Add(song);
            }

            albumData.AlbumTitle = albumTitle;
            albumData.Artist = artist;
            albumData.ImagePath = imagePath;
            albumData.Rating = rating;
            albumData.Songs = songs;

            this.data.SaveChanges();

            return true;
        }

        public bool EditSong(
                string id,
                string name,
                TimeSpan duration)
        {
            var song = this.data.Songs
                .Where(s => s.Id == id)
                .FirstOrDefault();

            if (song == null)
            {
                return false;
            }

            song.Name = name;
            song.SongDuration = duration;

            this.data.SaveChanges();

            return true;
        }
    }
}
