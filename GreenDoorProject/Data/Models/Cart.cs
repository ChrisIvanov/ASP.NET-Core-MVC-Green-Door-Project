namespace GreenDoorProject.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Cart
    {
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public ICollection<Book> Books { get; set; } = new List<Book>();

        public ICollection<Movie> Movies { get; set; } = new List<Movie>();

        public ICollection<Music> Music { get; set; } = new List<Music>();

        public ICollection<Game> Games { get; set; } = new List<Game>();

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
