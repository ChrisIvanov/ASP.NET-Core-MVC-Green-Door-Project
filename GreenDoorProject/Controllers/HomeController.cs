namespace GreenDoorProject.Controllers
{
    using System.Linq;
    using System.Diagnostics;
    using GreenDoorProject.Data;
    using GreenDoorProject.Data.Models;
    using GreenDoorProject.Infrastructure;
    using GreenDoorProject.Models;
    using GreenDoorProject.Services.Statistics;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly IStatisticsService statistics;
        private readonly GreenDoorProjectDbContext _data;

        public HomeController(
            IStatisticsService statistics,
            GreenDoorProjectDbContext data)
        {
            this.statistics = statistics;
            _data = data;
        }

        public IActionResult Index()
        {
            //TODO: Add a carousel with top quality merchandise

            var statistics = this.statistics.Total();

            return View(statistics);
        }


        [Authorize(Roles = "User, Member")]
        public IActionResult BecomePatron()
        {
            var userId = this.User.GetId();

            var user = this._data.Users
               .Where(u => u.Id == userId)
               .FirstOrDefault();

            var patron = new Patron
            {
                UserId = userId
            };

            this._data.Patrons.Add(patron);
            this._data.SaveChanges();

            return RedirectToAction("Home", "Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
