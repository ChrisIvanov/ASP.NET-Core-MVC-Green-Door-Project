namespace GreenDoorProject.Areas.Admin.Controllers
{
    using GreenDoorProject.Data;
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
    }
}
