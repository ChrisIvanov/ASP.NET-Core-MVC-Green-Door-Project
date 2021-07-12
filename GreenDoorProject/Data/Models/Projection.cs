namespace GreenDoorProject.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Projection
    {
        public int Id { get; init; }

        [Required]
        public string MoviePosterPath { get; set; }

        [Required]
        public string MovieTitle { get; set; }

        [Required]
        public DateTime TimeOfProjection { get; set; }

        public Hall CinemaHall { get; set; }

        public int AvailableSeats { get; set; } = 50;

        public string MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
