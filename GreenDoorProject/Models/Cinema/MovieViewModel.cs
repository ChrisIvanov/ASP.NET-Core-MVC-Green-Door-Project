using GreenDoorProject.Data.Models;
using System.Collections.Generic;

namespace GreenDoorProject.Models.Cinema
{
    public class MovieViewModel
    {
        public string Id { get; set; }

        public string MovieTitle { get; set; }

        public string Director { get; set; }

        public int YearOfRelease { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public string MovieDuration { get; set; }

        public double Rating { get; set; }

        public decimal TicketPrice { get; set; }

        public ICollection<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>();
    }
}
