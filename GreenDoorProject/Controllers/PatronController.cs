namespace GreenDoorProject.Controllers
{
    using System.Linq;
    using GreenDoorProject.Data;
    using GreenDoorProject.Data.Models;
    using GreenDoorProject.Infrastructure;
    using GreenDoorProject.Models.Patron;
    using GreenDoorProject.Services.Patrons;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class PatronController : Controller
    {
        private readonly GreenDoorProjectDbContext data;
        private readonly IPatronService patrons;

        public PatronController(
            GreenDoorProjectDbContext data, 
            IPatronService patrons)
        {
            this.data = data;
            this.patrons = patrons;
        }

        public IActionResult Donate()
            => View(new DonationsViewModel());

        [HttpPost]
        [Authorize]
        public IActionResult Donate(DonationsViewModel model)
        {
            var userId = this.User.GetId();

            decimal donations = model.DonationAmount;

            if (!patrons.IsPatron(userId))
            {
                var patron = new Patron
                {
                    UserId = userId,
                    Donations = model.DonationAmount,
                    Token = patrons.CalculateTokens(userId, donations)
                };

                this.data.Patrons.Add(patron);
            }
            else
            {
                var patron = this.data.Patrons
                .Where(p => p.UserId == userId)
                .FirstOrDefault();

                patron.Donations += model.DonationAmount;
                patron.Token += patrons.CalculateTokens(userId, donations);
            }

            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public IActionResult MyDonations()
        {
            var userId = User.GetId();

            if (patrons.IsPatron(userId))
            {
                var patron = this.data.Patrons
                .Where(p => p.UserId == userId)
                .FirstOrDefault();

                var userPatronage = new UserPatronageViewModel
                {
                    Tokens = patron.Token,
                    Donations = patron.Donations,
                    PatronSince = patron.PatronSince
                };

                return View(userPatronage);
            }

            return BadRequest();
        }

       
    }
}
