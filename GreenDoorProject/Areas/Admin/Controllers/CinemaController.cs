﻿namespace GreenDoorProject.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GreenDoorProject.Data;
    using GreenDoorProject.Data.Models;
    using GreenDoorProject.Infrastructure;
    using GreenDoorProject.Models.Cinema;
    using GreenDoorProject.Services.Movies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static AdminConstants;

    [Area(AdminAreaName)]
    [Authorize(Roles = AdministratorRoleName)]
    public class CinemaController : Controller
    {
        private readonly GreenDoorProjectDbContext data;
        private readonly IMovieService movies;

        public CinemaController(GreenDoorProjectDbContext data, IMovieService movies)
        {
            this.data = data;
            this.movies = movies;
        }

        public IActionResult AdminAll([FromQuery] AllMoviesQueryModel query)
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

        public IActionResult AdminAllActors()
        {
            if (!data.Actors.Any())
            {
                return RedirectToAction("AdminAll", "Cinema", new { area = "" });
            }

            var actors = this.data.Actors.ToList();

            var returnActors = new List<ActorViewModel>();

            foreach (var actor in actors)
            {
                returnActors.Add(new ActorViewModel
                {
                    Id = actor.Id,
                    FirstName = actor.FirstName,
                    LastName = actor.LastName,
                    ImagePath = actor.ImagePath,
                    YearOfBirth = actor.YearOfBirth,
                    YearOfDeath = actor.YearOfDeath,
                    Details = actor.Details
                });
            }

            return View(returnActors);
        }

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

            if (MovieExists(movieModel))
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
                ImagePath = movieModel.ImagePath,
                MovieDuration = new TimeSpan(movieDuration[0], movieDuration[1], 0),
                Description = movieModel.Description
            };

            this.data.Movies.Add(movie);
            this.data.SaveChanges();

            return RedirectToAction("AdminAll", "Cinema");
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

            var actorExists = this.data.Actors
                .Where(a => a.FirstName == actorModel.FirstName
                         && a.LastName == actorModel.LastName)
                .FirstOrDefault();

            if (actorExists != null)
            {
                ModelState.AddModelError(actorModel.FirstName + " " + actorModel.LastName, "Actor is already in the database.");
                return RedirectToAction("AdminAllActors", "Cinema");
            }

            var actor = new Actor
            {
                FirstName = actorModel.FirstName,
                LastName = actorModel.LastName,
                ImagePath = actorModel.ImagePath,
                YearOfBirth = actorModel.YearOfBirth,
                YearOfDeath = actorModel.YearOfDeath,
                Details = actorModel.Details
            };

            this.data.Actors.Add(actor);
            this.data.SaveChanges();

            return RedirectToAction("AdminAllActors", "Cinema");
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

        [Authorize]
        public IActionResult Edit(string id)
        {
            if (!User.IsAdmin())
            {
                ModelState.AddModelError
                    (string.Empty, "You are not authorized to make changes to the website content.");

                return RedirectToAction("AdminAll", "Cinema");
            }

            var movie = this.movies.Details(id);

            return View(new MovieFormModel
            {
                MovieTitle = movie.MovieTitle,
                Director = movie.Director,
                ImagePath = movie.ImagePath,
                YearOfRelease = movie.YearOfRelease,
                MovieDuration = movie.MovieDuration.ToString(),
                Rating = movie.Rating,
                Description = movie.Description
            });
        }

        [HttpPost]
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
                model.MovieDuration,
                model.Description);

            if (!edited)
            {
                return BadRequest();
            }

            return RedirectToAction("AdminAll", "Cinema");
        }

        [Authorize]
        public IActionResult Delete(string id)
        {
            var movie = this.data.Movies.Find(id);

            if (movie == null)
            {
                this.ModelState.AddModelError(nameof(CinemaController), "There is no movie with this Id in the database.");
            }
            else
            {
                var actors = this.data.ActorMovies
                    .Where(m => m.MovieId == movie.Id)
                    .ToList();

                if (actors.Any())
                {
                    foreach (var actor in actors)
                    {
                        this.data.ActorMovies.Remove(actor);
                    }
                }
                this.data.Remove(movie);
                this.data.SaveChanges();
            }

            return View("AdminAll", "Cinema");
        }

        public IActionResult DeleteActor(string id)
        {
            var actor = this.data.Actors.Find(id);

            if (actor == null)
            {
                ModelState.AddModelError(nameof(CinemaController), "Actor is not in the database.");
                return RedirectToAction("AdminAllActors", "Cinema");
            }

            var actorsMovies = this.data.ActorMovies
                .Where(m => m.ActorId == id)
                .ToList();

            if (actorsMovies.Any())
            {
                foreach (var actorMovie in actorsMovies)
                {
                    this.data.ActorMovies.Remove(actorMovie);
                }
            }

            this.data.Actors.Remove(actor);
            this.data.SaveChanges();

            return RedirectToAction("AdminAllActors", "Cinema");
        }

        private bool MovieExists(MovieFormModel movieModel)
            => this.data.Movies
                .Any(m => m.MovieTitle == movieModel.MovieTitle);
    }
}
