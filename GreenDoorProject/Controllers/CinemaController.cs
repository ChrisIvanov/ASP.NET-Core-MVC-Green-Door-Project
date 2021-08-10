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

    public class CinemaController : Controller
    {
        private readonly GreenDoorProjectDbContext data;
        private readonly IMovieService movies;

        public CinemaController(
            GreenDoorProjectDbContext data,
            IMovieService movies)
        {
            this.data = data;
            this.movies = movies;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Add()
            => View(new MovieFormModel());

        [HttpPost]
        [Authorize]
        public IActionResult Add(MovieFormModel movieModel)
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
            => View(new ActorFormModel());

        [HttpPost]
        [Authorize]
        public IActionResult AddActor(ActorFormModel actorModel)
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

        [Authorize]
        public IActionResult AddActorToMovie()
        {
            var actors = this.data.Actors
                .Select(a => new ActorViewModel
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName
                })
                .ToList();

            var movies = this.data.Movies
                .Select(m => new MovieViewModel
                {
                    Id = m.Id,
                    MovieTitle = m.MovieTitle
                })
                .ToList();

            return View(new AddActorToMovieFormModel
            {
                Actors = actors,
                Movies = movies
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddActorToMovie(AddActorToMovieFormModel model)
        {
            var actorMovie = new ActorMovie
            {
                ActorId = model.ActorId,
                MovieId = model.MovieId
            };

            this.data.ActorMovies.Add(actorMovie);
            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
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

        [Authorize]
        public IActionResult Edit(MovieFormModel model)
        {
            //if (!User.IsAdmin())
            //{
            //    return RedirectToAction("All", "Cinema");
            //}

            var movie = this.movies.Details("da1");

            return View(new MovieFormModel
            {
                MovieTitle = movie.MovieTitle,
                Director = movie.Director,
                ImagePath = movie.ImagePath,
                YearOfRelease = movie.YearOfRelease,
                MovieDuration = movie.MovieDuration,
                TicketPrice = movie.TicketPrice,
                Description = movie.Description,
            });
        }


        [HttpPut]
        [Authorize]
        public IActionResult Edit(MovieFormModel model, string id)
        {
            var movie = this.data.Movies
                .Where(m => m.Id == id)
                .FirstOrDefault();

            var edited = this.movies.Edit(
                id,
                model.MovieTitle,
                model.Director,
                model.ImagePath,
                model.YearOfRelease,
                model.TicketPrice,
                model.MovieDuration,
                model.Description);

            if (!edited)
            {
                return BadRequest();
            }

            return RedirectToAction("All", "Cinema");
        }

        [AllowAnonymous]
        public IActionResult PayPerView(string movieId)
        {
            var movie = GetMovie(movieId);

            return View(new BuyTicketsFormModel
            {
                PricePerTicket = movie.TicketPrice
            });
        }

        [HttpPut]
        public IActionResult PayPerView(BuyTicketsFormModel model)
        {
            var movie = GetMovie(model.Id);

            var projection = movie.Projections.Where(p => p.Id == model.ProjectionId).FirstOrDefault();

            var numberOfSeats = projection.AvailableSeats;

            if (numberOfSeats >= model.NumberOfTickets)
            {
                numberOfSeats -= model.NumberOfTickets;
            }

            return RedirectToAction("Index", "Home");
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

        private IEnumerable<Hall> GetHalls()
            => this.data.Halls.ToList();

        private bool ExistingMovieCheck(MovieFormModel movieModel)
            => this.data.Movies
                .Any(m => m.MovieTitle == movieModel.MovieTitle);

    }
}
