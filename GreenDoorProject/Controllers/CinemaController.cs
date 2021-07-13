namespace GreenDoorProject.Controllers
{
    using System.Linq;
    using System.Collections.Generic;
    using GreenDoorProject.Data;
    using GreenDoorProject.Models.Cinema;
    using Microsoft.AspNetCore.Mvc;

    public class CinemaController : Controller
    {
        private readonly GreenDoorProjectDbContext data;

        public CinemaController(GreenDoorProjectDbContext data)
        {
            this.data = data;
        }

        public IActionResult Add() => View();


        public IActionResult Add(AddMovieViewModel movie)
        {
            if (!ModelState.IsValid)
            {
                return View(movie);
            }

            return RedirectToAction("Index", "Home");
        }

        private IEnumerable<CinemaHallViewModel> GetCinemaHalls()
            => this.data
            .Halls
            .Select(h => new CinemaHallViewModel
            {
                Id = h.Id,
                Name = h.Name
            })
            .ToList();
    }
}
