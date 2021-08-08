namespace GreenDoorProject.Services.Books
{
    using System.Collections.Generic;
    using System.Linq;
    using GreenDoorProject.Data;
    using GreenDoorProject.Data.Models;
    using GreenDoorProject.Models.Books;
    using GreenDoorProject.Services.Books.Models;

    public class BookService : IBookService
    {
        private readonly GreenDoorProjectDbContext data;

        public BookService(GreenDoorProjectDbContext data)
            => this.data = data;

        public BookQueryServiceModel All(
            string genre,
            string searchTerm,
            BookSorting sorting,
            int currentPage,
            int booksPerPage)
        {
            var booksQuery = this.data.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(genre))
            {
                booksQuery = booksQuery.Where(b =>
                    b.Genre.Name == genre);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                booksQuery = booksQuery.Where(b =>
                    b.BookTitle.ToLower().Contains(searchTerm.ToLower())
                    || (b.Author.FirstName + " " + b.Author.LastName).Contains(searchTerm.ToLower())
                    || b.Description.Contains(searchTerm.ToLower())
                    || b.Genre.Name.Contains(searchTerm.ToLower()));
            }

            var totalBooks = booksQuery.Count();

            var books = booksQuery
                .Skip(((currentPage < 1 ? 1 : currentPage) - 1) * booksPerPage)
                .Take(booksPerPage)
                .OrderBy(a => a.Id)
                .Select(b => new BookServiceModel
                {
                    Id = b.Id,
                    BookTitle = b.BookTitle,
                    AuthorName = new string($"{b.Author.FirstName} {b.Author.LastName}"),
                    ImagePath = b.ImagePath,
                    Description = b.Description,
                    Genre = b.Genre,
                    Pages = b.Pages,
                    Rating = b.Rating
                })
                .ToList();

            booksQuery = sorting switch
            {
                BookSorting.BookTitleAscending => booksQuery.OrderBy(b => b.BookTitle),
                BookSorting.BookTitleDescending => booksQuery.OrderByDescending(b => b.BookTitle),
                BookSorting.AuthorNameAscending => booksQuery.OrderBy(b => b.Author.LastName).ThenBy(b => b.Author.FirstName),
                BookSorting.AuthorNameDescending => booksQuery.OrderByDescending(b => b.Author.LastName).ThenBy(b => b.Author.FirstName),
                BookSorting.RatingAscending => booksQuery.OrderByDescending(b => b.Rating.CurrentRating),
                BookSorting.RatingDescending => booksQuery.OrderByDescending(b => b.Rating.CurrentRating),
                _ => booksQuery.OrderByDescending(b => b.Id)
            };

            var bookGenres = this.data
                .Books
                .Select(b => b.Genre.Name)
                .Distinct()
                .ToList();

            return new BookQueryServiceModel
            {
                TotalBooks = totalBooks,
                CurrentPage = currentPage,
                BooksPerPage = booksPerPage,
                Books = books
            };
        }

        public BookServiceModel Details(string id)
        {
            var book = this.data.Books
                    .Where(b => b.Id == id)
                    .FirstOrDefault();

            var author = this.data.Authors
                .Where(a => a.Id == book.AuthorId)
                .Select(a => a.FirstName + " " + a.LastName)
                .FirstOrDefault();

            var genre = this.data.Genres
                .Where(g => g.Id == book.GenreId)
                .FirstOrDefault();

            var rating = this.data.Ratings
                .Where(r => r.BookId == id)
                .FirstOrDefault();

            var bookDetails = new BookServiceModel
            {
                Id = book.Id,
                BookTitle = book.BookTitle,
                AuthorName = author,
                Genre = genre,
                ImagePath = book.ImagePath,
                Pages = book.Pages,
                Rating = rating,
                Description = book.Description,
                Content = book.Content
            };

            return bookDetails;
        }

        public IEnumerable<string> AllBookGenres()
            => this.data.Genres
                .Select(g => g.Name)
                .ToList();

        public string GetAuthorId(string firstName, string lastName)
            => this.data.Authors
                .Where(a => a.FirstName == firstName &&
                        a.LastName == lastName)
                .Select(a => a.Id)
                .FirstOrDefault();

        public Author GetAuthor(string authorId)
            => this.data.Authors
                .Where(a => a.Id == authorId)
                .FirstOrDefault();
    }
}
