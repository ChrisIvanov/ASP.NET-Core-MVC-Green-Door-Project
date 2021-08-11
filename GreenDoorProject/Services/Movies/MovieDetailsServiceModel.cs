namespace GreenDoorProject.Services.Movies
{
    using System;
    using System.Collections.Generic;
    using GreenDoorProject.Data.Models;
    using GreenDoorProject.Models.Cinema;

    public class MovieDetailsServiceModel
    {
        public string Id { get; set; }

        public string MovieTitle { get; set; }

        public string Director { get; set; }

        public int YearOfRelease { get; set; }
        
        public string Description { get; set; }

        public string ImagePath { get; set; }

        public string MovieDuration { get; set; }

        public double? Rating { get; set; }

        public decimal TicketPrice { get; set; }

        public IEnumerable<ActorViewModel> Actors { get; set; }

    }
}
