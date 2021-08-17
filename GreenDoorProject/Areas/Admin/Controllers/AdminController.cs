namespace GreenDoorProject.Areas.Admin.Controllers
{
    using System.Linq;
    using GreenDoorProject.Areas.Admin.Models.Admin;
    using GreenDoorProject.Data;
    using GreenDoorProject.Infrastructure;
    using GreenDoorProject.Services.Statistics;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static AdminConstants;

    [Area(AdminAreaName)]
    [Authorize(Roles = AdministratorRoleName)]
    public class AdminController : Controller
    {
        private readonly IStatisticsService statistics;
        private readonly GreenDoorProjectDbContext data;

        public AdminController(
            IStatisticsService statistics,
            GreenDoorProjectDbContext data)
        {
            this.statistics = statistics;
            this.data = data;
        }

        public IActionResult Index()
        {
            var statistics = this.statistics.Total();

            return View(statistics);
        }

        public IActionResult AllUsers()
        {
            var guests = this.data.Users;
            var members = this.data.Members;
            var patrons = this.data.Patrons;

            var guestList = new AllUsers();

            foreach (var guest in guests)
            {
                var isMember = members.Any(m => m.UserId == guest.Id);
                var isPatron = patrons.Any(m => m.UserId == guest.Id);

                var currGuest = new UserDetails
                {
                    Id = guest.Id,
                    UserName = guest.UserName,
                    IsMemeber = isMember,
                    IsPatron = isPatron
                };

                guestList.Users.Add(currGuest);
            }

            return View();
        }

        [Authorize]
        public IActionResult RemoveUser(string userId)
        {
            var user = this.data.Users.Find(userId);

            if (user == null)
            {
                return BadRequest();
            }

            if (User.IsAdmin())
            {
                this.data.Remove(user);
            }

            this.data.SaveChanges();

            return RedirectToAction("Index", "Admin");
        }
    }
}
