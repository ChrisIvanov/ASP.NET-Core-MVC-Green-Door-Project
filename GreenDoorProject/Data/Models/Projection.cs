namespace GreenDoorProject.Data.Models
{
    using System;

    public class Projection
    {
        public int Id { get; init; }

        public string MoviePosterPath { get; set; }

        public string MovieTitle { get; set; }

        public DateTime TimeOfProjection { get; set; }

        public int AvailableSeats { get; set; }

        public string MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
