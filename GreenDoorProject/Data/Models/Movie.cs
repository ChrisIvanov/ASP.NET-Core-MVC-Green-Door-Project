namespace GreenDoorProject.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Movie
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public string MovieTitle { get; set; }

        public string Director { get; set; }

        public DateTime YearOfRelease { get; set; }

        public string Description { get; set; }

        public TimeSpan MovieDuration { get; set; }

        public decimal TicketPrice { get; set; }

        public ICollection<Projection> Projections { get; init; } = new List<Projection>();

        public ICollection<Actor> Actors { get; set; } = new List<Actor>();
    }
}
