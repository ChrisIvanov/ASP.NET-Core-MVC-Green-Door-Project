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
            int booksPerPage,
            bool showOnlyAuthors);

        BookServiceModel Details(string id);

        public bool Edit(string id,
                string bookTitle,
                string authorFirstName,
                string authorLastName,
                string imagePath,
                int pages,
                double rating,
                string description,
                byte[] content);

        public bool EditAuhtor(string id,
                string firstName,
                string lastName,
                string imagePath,
                int yearOfBirth,
                int yearOfDeath,
                string details);

        IEnumerable<string> AllBookGenres();

        string GetAuthorId(string firstName, string lastName);

        Author GetAuthor(string authorId);

    }
}
