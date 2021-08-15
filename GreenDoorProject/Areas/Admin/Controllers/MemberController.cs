namespace GreenDoorProject.Areas.Admin.Controllers
{
    using GreenDoorProject.Data;
    using GreenDoorProject.Infrastructure;
    using GreenDoorProject.Services.Members;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    
    public class MemberController : Controller
    {
        private readonly GreenDoorProjectDbContext data;
        private readonly IMemberService members;

        public MemberController(
            GreenDoorProjectDbContext data, 
            IMemberService members)
        {
            this.data = data;
            this.members = members;
        }

        [Authorize]
        public IActionResult RemoveMember(string userId)
        {
            if (User.IsAdmin())
            {
                members.RemoveMember(userId);
            }

            return RedirectToAction("Index", "Admin");
        }
    }
}
