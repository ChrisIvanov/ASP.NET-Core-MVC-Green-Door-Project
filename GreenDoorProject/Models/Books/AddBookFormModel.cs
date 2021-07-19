namespace GreenDoorProject.Models.Books
{
    using GreenDoorProject.Data.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class AddBookFormModel
    {
        [Required]
        [MinLength(BookTitleMinLength)]
        [MaxLength(BookTitleMaxLength)]
        [Display(Name = "Title")]
        public string BookTitle { get; set; }

        [Required]
        [Range(BookPagesMinLength, BookPagesMaxLength)]
        [Display(Name = "Number of pages")]
        public int Pages { get; set; }

        [Display(Name = "Image Path")]
        public string ImagePath { get; set; }

        [Required]
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        public IEnumerable<BookGenreViewModel> Genres { get; set; }

        [Required]
        [Range(0.01, 500.0)]
        public decimal Price { get; set; }

        [MaxLength(DefaultDescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [MinLength(AuthorNameMinLength)]
        [MaxLength(AuthorNameMaxLength)]
        [Display(Name = "Author's First Name")]
        public string AuthorFirstName { get; set; }

        [Required]
        [MinLength(AuthorNameMinLength)]
        [MaxLength(AuthorNameMaxLength)]
        [Display(Name = "Author's Last Name")]
        public string AuthorLastName { get; set; }
    }
}
