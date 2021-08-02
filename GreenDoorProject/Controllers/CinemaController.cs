namespace GreenDoorProject.Controllers
{
    using System.Linq;
    using System.Collections.Generic;
    using GreenDoorProject.Data;
    using GreenDoorProject.Models.Cinema;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using GreenDoorProject.Services.Movies;

    public class CinemaController : Controller
    {
        private readonly GreenDoorProjectDbContext data;
        private readonly IMoviesService movies;

        public CinemaController(GreenDoorProjectDbContext data)
        {
            this.data = data;
        }

        [HttpGet]
        [Authorize(Roles = "User, Member, Admin")]
        public IActionResult All([FromQuery] AllMoviesQueryModel query)
        {
            if (!data.Movies.Any())
            {
                var error = "Currently there are no books in the library.";

                var errorModel = new AllMoviesQueryModel
                {
                    ModelError = error
                };

                return View(errorModel);
            }

            var queryResult = this.movies.All(
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllMoviesQueryModel.MoviesPerPage);

            var totalMovies = query.TotalMovies;

            query.Movies = queryResult.Movies;
            query.TotalMovies = totalMovies;

            return View(query);
        }

        [HttpGet]
        [Authorize(Roles = "User, Member, Admin")]
        public IActionResult Details(string movieId)
        {
            var movie = this.movies.Details(movieId);

            return View(movie);
        }

        private IEnumerable<CinemaHallViewModel> GetCinemaHalls()
            => this.data.Halls
            .Select(h => new CinemaHallViewModel
            {
                Id = h.Id,
                Name = h.Name
            })
            .ToList();
    }
}
