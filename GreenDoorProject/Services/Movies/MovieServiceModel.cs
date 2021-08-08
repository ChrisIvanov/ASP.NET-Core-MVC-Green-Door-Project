namespace GreenDoorProject.Services.Movies
{
    using System;
    using GreenDoorProject.Data.Models;

    public class MovieServiceModel
    {
        public string Id { get; set; }

        public string MovieTitle { get; set; }

        public string Director { get; set; }

        public int YearOfRelease { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public TimeSpan MovieDuration { get; set; }

        public Rating Rating { get; set; }

        public decimal TicketPrice { get; set; }
    }
}
