namespace GreenDoorProject.Controllers
{
    using System.Diagnostics;
    using GreenDoorProject.Data;
    using GreenDoorProject.Models;
    using GreenDoorProject.Services.Statistics;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly IStatisticsService statistics;
        private readonly GreenDoorProjectDbContext data;

        public HomeController(
            IStatisticsService statistics,
            GreenDoorProjectDbContext data)
        {
            this.statistics = statistics;
            this.data = data;
        }

        public IActionResult Index()
        {
            //TODO: Add a carousel with top quality merchandise

            var statistics = this.statistics.Total();

            return View(statistics);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
