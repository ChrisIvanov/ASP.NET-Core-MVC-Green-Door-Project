namespace GreenDoorProject.Controllers
{
    using GreenDoorProject.Data;
    using GreenDoorProject.Data.Models;
    using GreenDoorProject.Infrastructure;
    using GreenDoorProject.Models.Patron;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    public class PatronController : Controller
    {
        private readonly GreenDoorProjectDbContext data;

        public PatronController(GreenDoorProjectDbContext data)
        {
            this.data = data;
        }

        public IActionResult Donate()
            => View(new DonationsViewModel());

        [HttpPost]
        [Authorize]
        public IActionResult Donate(DonationsViewModel model)
        {
            var userId = this.User.GetId();

            var patron = this.data.Patrons
                .Where(p => p.UserId == userId)
                .FirstOrDefault();

            decimal donations = model.DonationAmount;
            int tokens = 0;

            if (donations >= 5 && donations < 10) tokens = 2;
            else if (donations >= 10 && donations < 20) tokens = 5;
            else if (donations >= 20 && donations < 30) tokens = 12;
            else if (donations >= 30 && donations < 40) tokens = 20;
            else if (donations >= 40 && donations < 50) tokens = 35;
            else if (donations >= 50 && donations < 60) tokens = 45;

            if (patron == null)
            {
                var patorn = new Patron
                {
                    UserId = userId,
                    Donations = model.DonationAmount,
                    Token = tokens
                };
            }
            else
            {
                patron.Donations += model.DonationAmount;
                patron.Token += tokens;
            }

            return RedirectToAction();
        }

        [HttpGet]
        [Authorize]
        public IActionResult MyDonations()
        {
            var userId = this.User.GetId();

            var patron = this.data.Patrons
                .Where(p => p.UserId == userId)
                .FirstOrDefault();

            var userPatronage = new UserPatronageViewModel
            {
                Tokens = patron.Token,
                Donations = patron.Donations
            };

            return View(userPatronage);
        }

        [HttpPut]
        [Authorize]
        public string UseToken()
        {
            var userId = this.User.GetId();

            var patron = this.data.Patrons
                .Where(p => p.UserId == userId)
                .FirstOrDefault();

            if (patron.Token > 0)
            {
                patron.Token -= 1;
                return "You have used 1 token. Have a pleasant day.";
            }
            else
            {
                return "You have no tokens left. Consider becoming a member.";
            }
        }
    }
}
