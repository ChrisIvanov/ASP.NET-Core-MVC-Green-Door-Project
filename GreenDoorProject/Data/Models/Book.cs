namespace GreenDoorProject.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Book;
    using static DataConstants.Info;

    public class Book
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string BookTitle { get; set; }

        [Required]
        [Range(PagesMinLength, PagesMaxLength)]
        public int Pages { get; set; }

        [Url]
        [Required]
        public string ImagePath { get; set; }

        [StringLength(DefaultClassInfoMaxLength)]
        public string Description { get; set; }

        [Required]
        [Range(0,10)]
        public double Rating { get; set; }

        [Required]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        [Required]
        public byte[] Contents { get; set; }

        [Required]
        public string AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
