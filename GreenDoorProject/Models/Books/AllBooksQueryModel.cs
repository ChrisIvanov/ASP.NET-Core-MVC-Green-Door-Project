namespace GreenDoorProject.Models.Books
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using GreenDoorProject.Services.Books.Models;

    public class AllBooksQueryModel
    {
        public string Genre { get; set; }

        public IEnumerable<string> Genres { get; set; }

        [Display(Name = "Search")]
        public string SearchTerm { get; set; }

        public BookSorting Sorting { get; set; }

        public bool ShowOnlyAuthors { get; set; }

        public int CurrentPage { get; set; }

        public const int BooksPerPage = 20;

        public string ModelError { get; set; }

        public int TotalBooks { get; set; }
        
        public IEnumerable<BookServiceModel> Books { get; set; }

        public IEnumerable<AuthorDetailsViewModel> Authors { get; set; }
    }
}
