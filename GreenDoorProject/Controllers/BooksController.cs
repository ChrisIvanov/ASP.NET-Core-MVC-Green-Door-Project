namespace GreenDoorProject.Controllers
{
    using System.Linq;
    using GreenDoorProject.Data;
    using GreenDoorProject.Infrastructure;
    using GreenDoorProject.Models.Books;
    using GreenDoorProject.Services.Books;
    using GreenDoorProject.Services.Patrons;
    using Microsoft.AspNetCore.Mvc;

    public class BooksController : Controller
    {
        private readonly IBookService books;
        private readonly IPatronService patrons;
        private readonly GreenDoorProjectDbContext data;

        public BooksController(
            GreenDoorProjectDbContext data, 
            IBookService books)
        {
            this.data = data;
            this.books = books;
        }

        [HttpGet]
        public IActionResult All([FromQuery]AllBooksQueryModel query)
        {
            if (!data.Books.Any())
            {
                var error = "Currently there are no books in the library.";

                var errorModel = new AllBooksQueryModel
                {
                    ModelError = error
                };

                return View(errorModel);
            }

            var queryResult = this.books.All(
                query.Genre,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllBooksQueryModel.BooksPerPage);

            var bookGenres = this.books.AllBookGenres();

            var totalBooks = query.TotalBooks;

            query.Genres = bookGenres;
            query.Books = queryResult.Books;
            query.TotalBooks = totalBooks;

            return View(query);
        }

        public IActionResult Details(string bookId)
        {
            var book = this.books.Details(bookId);

            return View(book);
        }

        public IActionResult ReadAsMember(string bookId)
        {
            var books = this.data.Books.AsQueryable();

            var book = books
                .Where(b => b.Id == bookId)
                .Select(b => new BookContentsViewModel
                {   
                    Title = b.BookTitle,
                    Author = b.Author.FirstName + " " + b.Author.LastName,
                    Contents = b.Content
                })
                .FirstOrDefault();

            return View(book);
        }

        public IActionResult ReadAsPatron(string bookId)
        {
            this.patrons.UseToken(User.GetId());

            var books = this.data.Books.AsQueryable();

            var book = books
                .Where(b => b.Id == bookId)
                .Select(b => new BookContentsViewModel
                {
                    Title = b.BookTitle,
                    Author = b.Author.FirstName + " " + b.Author.LastName,
                    Contents = b.Content
                })
                .FirstOrDefault();

            return View(book);
        }
    }
}
