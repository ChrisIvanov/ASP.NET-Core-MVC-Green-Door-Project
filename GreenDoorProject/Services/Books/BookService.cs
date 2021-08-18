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
            int booksPerPage,
            bool showOnlyAuthors)
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
                    AuthorFirstName = b.Author.FirstName,
                    AuthorLastName = b.Author.LastName,
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
                BookSorting.RatingAscending => booksQuery.OrderByDescending(b => b.Rating),
                BookSorting.RatingDescending => booksQuery.OrderByDescending(b => b.Rating),
                _ => booksQuery.OrderByDescending(b => b.Id)
            };

            var bookGenres = this.data
                .Books
                .Select(b => b.Genre.Name)
                .Distinct()
                .ToList();

            var authors = new List<AuthorDetailsViewModel>();

            if (showOnlyAuthors)
            {
                authors = this.data.Authors.Select(a => new AuthorDetailsViewModel
                {
                    FullName = a.FirstName + " " + a.LastName,
                    ImagePath = a.ImagePath,
                    YearOfBirth = a.YearOfBirth,
                    YearOfDeath = a.YearOfDeath,
                    Details = a.Details
                })
                .ToList();
            }

            return new BookQueryServiceModel
            {
                TotalBooks = totalBooks,
                CurrentPage = currentPage,
                BooksPerPage = booksPerPage,
                ShowOnlyAuthors = showOnlyAuthors,
                Books = books,
                Authors = authors
            };
        }

        public BookServiceModel Details(string id)
        {
            var book = this.data.Books
                    .Where(b => b.Id == id)
                    .FirstOrDefault();

            var author = this.data.Authors
                .Where(a => a.Id == book.AuthorId)
                .FirstOrDefault();

            var genre = this.data.Genres
                .Where(g => g.Id == book.GenreId)
                .FirstOrDefault();

            var bookDetails = new BookServiceModel
            {
                Id = book.Id,
                BookTitle = book.BookTitle,
                AuthorFirstName = author.FirstName,
                AuthorLastName = author.LastName,
                Genre = genre,
                ImagePath = book.ImagePath,
                Pages = book.Pages,
                Rating = book.Rating,
                Description = book.Description,
                Contents = book.Content
            };

            return bookDetails;
        }

        public AuthorDetailsViewModel AuthorDetails(string id)
        {
            var author = this.data.Authors
                .Where(a => a.Id == id)
                .FirstOrDefault();

            var authorDetails = new AuthorDetailsViewModel
            {
                Id = author.Id,
                FullName = author.FirstName + " " + author.LastName,
                ImagePath = author.ImagePath,
                YearOfBirth = author.YearOfBirth,
                YearOfDeath = author.YearOfDeath,
                Details = author.Details,
                AuthorBooks = author.AuthorBooks
            };

            return authorDetails;
        }

        public bool Edit(string id,
                string bookTitle,
                string authorFirstName,
                string authorLastName,
                string imagePath,
                int pages,
                double rating,
                string description,
                byte[] content)
        {
            var bookData = this.data.Books.Find(id);

            if (bookData == null)
            {
                return false;
            }

            bookData.BookTitle = bookTitle;
            bookData.Author.FirstName = authorFirstName;
            bookData.Author.LastName = authorLastName;
            bookData.ImagePath = imagePath;
            bookData.Pages = pages;
            bookData.Rating = rating;
            bookData.Description = description;
            bookData.Content = content;

            this.data.SaveChanges();

            return true;
        }

        public bool EditAuhtor(
            string id,
            string firstName,
            string lastName,
            string imagePath,
            int yearOfBirth,
            int yearOfDeath,
            string details)
        {
            var authorData = this.data.Authors.Find(id);

            if (authorData == null)
            {
                return false;
            }

            authorData.FirstName = firstName;
            authorData.LastName = lastName;
            authorData.ImagePath = imagePath;
            authorData.YearOfBirth = yearOfBirth;
            authorData.YearOfDeath = yearOfDeath;
            authorData.Details = details;

            this.data.SaveChanges();

            return true;
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
