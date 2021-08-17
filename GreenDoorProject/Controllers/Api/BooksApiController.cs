namespace GreenDoorProject.Controllers.Api
{
    using GreenDoorProject.Models.Api.Books;
    using GreenDoorProject.Services.Books;
    using GreenDoorProject.Services.Books.Models;
    using Microsoft.AspNetCore.Mvc;
    
    [ApiController]
    [Route("api/books")]
    public class BooksApiController : ControllerBase
    {
        private readonly IBookService books;

        public BooksApiController(IBookService books) 
            => this.books = books;

        public BookQueryServiceModel All(
            [FromQuery] AllBooksApiRequestModel query) 
            => this.books.All(
                query.Genre,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                query.BooksPerPage,
                query.ShowOnlyAuthors);
    }
}
