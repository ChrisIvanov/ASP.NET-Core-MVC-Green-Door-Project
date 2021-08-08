using System;
using System.Collections.Generic;

namespace GreenDoorProject.Data.Models
{    
    public class Rating
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public double CurrentRating { get; set; } = 0.00;

        public int CurrentVotesCount { get; set; }

        public int UserRating { get; set; }

        public bool UserHasVoted { get; set; }

        public string BookId { get; set; }
        public Book Book { get; set; }

        public string MovieId { get; set; }
        public Movie Movie { get; set; }

        public string MusicAlbumId { get; set; }
        public MusicAlbum MusicAlbum { get; set; }
    }
}
