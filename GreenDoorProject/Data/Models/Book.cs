namespace GreenDoorProject.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Book;
    
    public class Book
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string BookTitle { get; set; }

        [Required]
        [Range(PagesMinLength, PagesMaxLength)]
        public int Pages { get; set; }

        public string ImagePath { get; set; }

        [Required]
        [Range(0.05, 500.0)]
        public decimal Price { get; set; }

        public string Description { get; set; }

        [Range(0, 5)]
        public double Rating { get; set; }

        [Required]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
