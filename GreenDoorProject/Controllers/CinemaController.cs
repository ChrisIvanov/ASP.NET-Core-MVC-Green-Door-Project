namespace GreenDoorProject.Controllers
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using GreenDoorProject.Data;
    using GreenDoorProject.Models.Cinema;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using GreenDoorProject.Services.Movies;
    using GreenDoorProject.Data.Models;
    using GreenDoorProject.Infrastructure;
    using GreenDoorProject.Services.Patrons;
    using GreenDoorProject.Services.Members;

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
        public IActionResult WatchMovie(string movieId)
        {
            var movie = GetMovie(movieId);

            var userId = this.User.GetId();

            if (members.IsMember(userId))
            {
                return RedirectToAction("WatchMovie", "Cinema");
            }
            else
            {
                if (patrons.IsPatron(userId))
                {
                    if (this.patrons.GetTokens(userId) < 1)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    this.patrons.UseToken(userId);

                    return View(movie);
                }
            }

            return RedirectToAction("BecomeMember", "Member");
        }

        
        private MovieViewModel GetMovie(string movieId)
        {
            var getMovie = this.data.Movies
                 .Where(m => m.Id == movieId)
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
