using GreenDoorProject.Models.Music;
using GreenDoorProject.Services.Music.Models;
using System;
using System.Collections.Generic;

namespace GreenDoorProject.Services.Music
{

    public interface IMusicService
    {
        MusicQueryServiceModel All(
            string searchTerm,
            MusicSorting sorting,
            int currentPage,
            int booksPerPage,
            bool showOnlySongs);

        MusicServiceModel Details(string id);

        bool Edit(string id,
                string title,
                string artist,
                string imagePath,
                double rating,
                IEnumerable<SongsDetailsViewModel> songs);

        bool EditSong(string id,
                string name,
                TimeSpan duration);
    }
}
