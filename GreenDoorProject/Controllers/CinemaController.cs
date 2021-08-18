namespace GreenDoorProject.Controllers
{
    using System.Linq;
    using GreenDoorProject.Data;
    using GreenDoorProject.Infrastructure;
    using GreenDoorProject.Models.Cinema;
    using GreenDoorProject.Services.Movies;
    using GreenDoorProject.Services.Members;
    using GreenDoorProject.Services.Patrons;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using GreenDoorProject.Services.Movies.Models;

    public class CinemaController : Controller
    {
        private readonly GreenDoorProjectDbContext data;
        private readonly IMovieService movies;
        private readonly IMemberService members;
        private readonly IPatronService patrons;

        public CinemaController(
            GreenDoorProjectDbContext data,
            IMovieService movies, 
            IMemberService members, 
            IPatronService patrons)
        {
            this.data = data;
            this.movies = movies;
            this.members = members;
            this.patrons = patrons;
        }
        [HttpGet]
        [Authorize]
        public IActionResult All([FromQuery] AllMoviesQueryModel query)
        {
            if (!data.Movies.Any())
            {
                var error = "Currently there are no movies in the video library.";

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
        [Authorize]
        public IActionResult Details([FromRoute]string id)
        {
            var movie = this.movies.Details(id);

            return View(movie);
        }

        [HttpGet]
        [Authorize]
        public IActionResult WatchMovie(string id)
        {
            var userId = this.User.GetId();

            if (members.IsMember(userId))
            {
                return View();
            }
            else
            {
                if (patrons.IsPatron(userId))
                {
                    if (this.patrons.GetTokens(userId).Tokens < 1)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    this.patrons.UseToken(userId);

                    return View();
                }
            }

            return RedirectToAction("BecomeMember", "Member");
        }

        
        private MovieViewModel GetMovie(string id)
        {
            var getMovie = this.data.Movies
                 .Where(m => m.Id == id)
                 .FirstOrDefault();

            var movie = new MovieViewModel
            {
                Id = getMovie.Id,
                MovieTitle = getMovie.MovieTitle,
                Director = getMovie.Director,
                ImagePath = getMovie.ImagePath,
                MovieDuration = getMovie.MovieDuration.ToString(),
                Description = getMovie.Description,
                Rating = getMovie.Rating,
                YearOfRelease = getMovie.YearOfRelease,
                ActorMovies = getMovie.ActorMovies
            };

            return movie;
        }
    }
}
