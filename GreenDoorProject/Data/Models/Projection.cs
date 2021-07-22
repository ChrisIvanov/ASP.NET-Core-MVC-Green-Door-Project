namespace GreenDoorProject.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Projection
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string MoviePosterPath { get; set; }

        [Required]
        public string MovieTitle { get; set; }

        [Required]
        public DateTime TimeOfProjection { get; set; }

        [Required]
        [Range(0, 50)]
        public int AvailableSeats { get; set; } = 50;

        [Required]
        public int HallId { get; set; }
        public Hall Hall { get; set; }

        [Required]
        public string AdminId { get; init; }
        public Admin Admin { get; init; }

        [Required]
        public string MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
