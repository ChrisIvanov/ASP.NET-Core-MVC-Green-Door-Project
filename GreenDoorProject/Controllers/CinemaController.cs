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

    public class CinemaController : Controller
    {
        private readonly GreenDoorProjectDbContext data;
        private readonly IMoviesService movies;

        public CinemaController(GreenDoorProjectDbContext data)
        {
            this.data = data;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Add()
            => View(new AddMovieFormModel());

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddMovieFormModel movieModel)
        {
            if (!ModelState.IsValid)
            {
                return View(movieModel);
            }

            if (ExistingMovieCheck(movieModel))
            {
                this.ModelState
                    .AddModelError(
                        nameof(movieModel.MovieTitle),
                        "The movie already exists.");
            }

            var movieDuration = movieModel.MovieDuration
                .Split(":", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            var movie = new Movie
            {
                MovieTitle = movieModel.MovieTitle,
                Director = movieModel.Director,
                YearOfRelease = movieModel.YearOfRelease,
                TicketPrice = movieModel.TicketPrice,
                MovieDuration = new TimeSpan(movieDuration[0], movieDuration[1], 0),
                Description = movieModel.Description
            };

            this.data.Movies.Add(movie);
            this.data.SaveChanges();

            return RedirectToAction("Add", "Cinema");
        }

        [Authorize]
        public IActionResult AddActor() 
            => View(new AddActorViewModel());

        [HttpPost]
        [Authorize]
        public IActionResult AddActor(AddActorViewModel actorModel)
        {
            if (!ModelState.IsValid)
            {
                return View(actorModel);
            }

            var actor = new Actor
            {
                FirstName = actorModel.FirstName,
                LastName = actorModel.LastName,
                YearOfBirth = actorModel.YearOfBirth,
                YearOfDeath = actorModel.YearOfDeath,
                Details = actorModel.Details
            };

            this.data.Actors.Add(actor);
            this.data.SaveChanges();

            return RedirectToAction("AddActor", "Cinema");
        }

        [HttpGet]
        [Authorize]
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
        [Authorize]
        public IActionResult Details(string movieId)
        {
            var movie = this.movies.Details(movieId);

            return View(movie);
        }

        [AllowAnonymous]
        public IActionResult Buy(string movieId)
        {
            var movie = GetMovie(movieId);

            return View(new BuyTicketsFormModel
            {
                PricePerTicket = movie.TicketPrice
            });
        }

        [HttpPut]
        public IActionResult Buy(BuyTicketsFormModel model)
        {
            var movie = GetMovie(model.Id);

            var projection = movie.Projections.Where(p => p.Id == model.ProjectionId).FirstOrDefault();

            var numberOfSeats = projection.AvailableSeats;

            if (numberOfSeats >= model.NumberOfTickets)
            {
                numberOfSeats -= model.NumberOfTickets;
            }

            return RedirectToAction("Home", "Index");
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
                TicketPrice = getMovie.TicketPrice,
                MovieDuration = getMovie.MovieDuration.ToString(),
                Description = getMovie.Description,
                Rating = getMovie.Rating.CurrentRating.ToString(),
                YearOfRelease = getMovie.YearOfRelease,
                ActorMovies = getMovie.ActorMovies,
                Projections = getMovie.Projections
            };

            return movie;
        }

        private IEnumerable<CinemaHallViewModel> GetCinemaHalls()
            => this.data.Halls
            .Select(h => new CinemaHallViewModel
            {
                Id = h.Id,
                Name = h.Name
            })
            .ToList();

        private bool ExistingMovieCheck(AddMovieFormModel movieModel)
            => this.data.Movies
                .Any(m => m.MovieTitle == movieModel.MovieTitle);

        private IEnumerable<Hall> GetHalls()
            => this.data.Halls.ToList();
    }
}
