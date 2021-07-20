namespace GreenDoorProject.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class Admin
    {
        public string Id { get; init; }

        public int UserId { get; set; }

        public IdentityUser User { get; set; }

        public IEnumerable<Book> Books { get; set; } = new List<Book>();

        public IEnumerable<Author> Authors { get; set; } = new List<Author>();

        public IEnumerable<Movie> Movies { get; set; } = new List<Movie>();

        public IEnumerable<Actor> Actors { get; set; } = new List<Actor>();

        public IEnumerable<Projection> Projections { get; set; } = new List<Projection>();

        public IEnumerable<Game> Games { get; set; } = new List<Game>();
        
        public IEnumerable<Music> Music { get; set; } = new List<Music>();

        public IEnumerable<Song> Song { get; set; } = new List<Song>();

    }
}
