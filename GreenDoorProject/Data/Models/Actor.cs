namespace GreenDoorProject.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Actor
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<Movie> ActorMovies { get; set; } = new List<Movie>();
    }
}