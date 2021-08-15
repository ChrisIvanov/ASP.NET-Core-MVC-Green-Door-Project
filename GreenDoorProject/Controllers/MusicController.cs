namespace GreenDoorProject.Controllers
{
    using GreenDoorProject.Data;
    using Microsoft.AspNetCore.Mvc;
    
    public class MusicController : Controller
    {
        private readonly GreenDoorProjectDbContext data;

        public MusicController(GreenDoorProjectDbContext data)
        {
            this.data = data;
        }


    }
}
