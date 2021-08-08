namespace GreenDoorProject.Services.Books
{
    using System.Collections.Generic;
    using GreenDoorProject.Data.Models;
    using GreenDoorProject.Models.Books;
    using GreenDoorProject.Services.Books.Models;

    public interface IBookService
    {
        BookQueryServiceModel All(
            string genre,
            string searchTerm,
            BookSorting sorting,
            int currentPage,
            int booksPerPage);

        BookServiceModel Details(string id);

        IEnumerable<string> AllBookGenres();

        string GetAuthorId(string firstName, string lastName);

        Author GetAuthor(string authorId);

    }
}
