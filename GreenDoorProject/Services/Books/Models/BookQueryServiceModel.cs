namespace GreenDoorProject.Services.Books.Models
{
    using System.Collections.Generic;
    
    public class BookQueryServiceModel
    {
        public int BooksPerPage { get; init; }

        public int CurrentPage { get; init; }

        public int TotalBooks { get; init; }

        public string ModelError { get; init; }

        public IEnumerable<BookServiceModel> Books { get; init; }
    }
}
