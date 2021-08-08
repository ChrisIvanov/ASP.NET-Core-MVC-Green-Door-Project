namespace GreenDoorProject.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GreenDoorProject.Data;
    using GreenDoorProject.Data.Models;
    using GreenDoorProject.Models.Cinema;
    using Microsoft.AspNetCore.Mvc;

    public class CinemaController : AdminController
    {
        private readonly GreenDoorProjectDbContext data;

        public CinemaController(GreenDoorProjectDbContext data)
        {
            this.data = data;
        }

        public IActionResult Add()
            => View(new MovieFormModel());

        [HttpPost]
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

            return RedirectToAction("Cinema", "All");
        }

        private bool ExistingMovieCheck(MovieFormModel movieModel)
            => this.data.Movies
                .Any(m => m.MovieTitle == movieModel.MovieTitle);

        private IEnumerable<Hall> GetHalls()
            => this.data.Halls.ToList();
    }
}
