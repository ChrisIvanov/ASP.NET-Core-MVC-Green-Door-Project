namespace GreenDoorProject.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using GreenDoorProject.Data;
    using GreenDoorProject.Data.Models;
    using GreenDoorProject.Infrastructure;
    using GreenDoorProject.Models;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly GreenDoorProjectDbContext _data;

        public HomeController(GreenDoorProjectDbContext data)
        {
            _data = data;
        }

        public IActionResult Index() => View();

        public IActionResult BecomeAdmin()
        {
            var userId = this.User.GetId();

            var user = this._data.Users
               .Where(u => u.Id == userId)
               .FirstOrDefault();

            var admin = new Admin
            {
                UserId = userId
            };

            this._data.Admins.Add(admin);
            this._data.SaveChanges();

            return RedirectToAction("Home", "Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
