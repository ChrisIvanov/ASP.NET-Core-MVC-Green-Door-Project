namespace GreenDoorProject.Data.Models
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

    public class Admin
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public string UserId { get; set; }

        public IEnumerable<Actor> Actors { get; set; } = new List<Actor>();

        public IEnumerable<Author> Authors { get; set; } = new List<Author>();

        public IEnumerable<Book> Books { get; set; } = new List<Book>();

        public IEnumerable<Game> Games { get; set; } = new List<Game>();

        public IEnumerable<Movie> Movies { get; set; } = new List<Movie>();

        public IEnumerable<MusicAlbum> MusicAlbums { get; set; } = new List<MusicAlbum>();
        
        public IEnumerable<Projection> Projections { get; set; } = new List<Projection>();

        public IEnumerable<Song> Songs { get; set; } = new List<Song>();

    }
}
