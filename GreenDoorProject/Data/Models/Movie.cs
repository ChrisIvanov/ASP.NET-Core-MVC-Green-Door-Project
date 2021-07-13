namespace GreenDoorProject.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Movie
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string MovieTitle { get; set; }

        [Required]
        public string Director { get; set; }

        [Required]
        public int YearOfRelease { get; set; }

        public string Description { get; set; }

        public TimeSpan MovieDuration { get; set; }

        [Required]
        public decimal TicketPrice { get; set; }

        public ICollection<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>();

        public ICollection<Projection> Projections { get; init; } = new List<Projection>();
    }
}
