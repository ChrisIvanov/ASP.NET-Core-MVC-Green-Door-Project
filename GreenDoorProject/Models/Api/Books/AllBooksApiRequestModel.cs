namespace GreenDoorProject.Models.Api.Books
{
    using GreenDoorProject.Models.Books;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AllBooksApiRequestModel
    {
        public string Genre { get; set; }

        public IEnumerable<string> Genres { get; set; }

        [Display(Name = "Search by term")]
        public string SearchTerm { get; set; }

        public BookSorting Sorting { get; set; }

        public int CurrentPage { get; set; }

        public int BooksPerPage { get; set; }

        public string ModelError { get; set; }

        public int TotalBooks { get; set; }
    }
}
