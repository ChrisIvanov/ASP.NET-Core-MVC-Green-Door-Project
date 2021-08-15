namespace GreenDoorProject.Areas.Guest.Controllers
{
    using GreenDoorProject.Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Guest.GuestConstants;

    [Area(GuestAreaName)]
    [Authorize(Roles = GuestRoleName)]
    public class GuestController : Controller
    {
        private readonly GreenDoorProjectDbContext data;

        public GuestController(GreenDoorProjectDbContext data)
        {
            this.data = data;
        }
    }
}
