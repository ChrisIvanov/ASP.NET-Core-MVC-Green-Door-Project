namespace GreenDoorProject.Models.Books
{
    using GreenDoorProject.Data.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Author;
    using static Data.DataConstants.Book;
    using static Data.DataConstants.Info;

    public class AddBookFormModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        [Display(Name = "Title")]
        public string BookTitle { get; set; }

        [Required]
        [Range(PagesMinLength, PagesMaxLength)]
        [Display(Name = "Number of pages")]
        public int Pages { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        [Display(Name = "Author's First Name")]
        public string AuthorFirstName { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        [Display(Name = "Author's Last Name")]
        public string AuthorLastName { get; set; }

        [Url]
        [Required]
        [Display(Name = "Image URL")]
        public string ImagePath { get; set; }

        public double Rating { get; set; }

        [Required]
        [Display(Name = "Genre")]
        public int GenreId { get; set; }
        public IEnumerable<BookGenreViewModel> Genres { get; set; }

        [MaxLength(DefaultClassInfoMaxLength)]
        public string Description { get; set; }

        [Required]
        public byte Content { get; set; }

    }
}
