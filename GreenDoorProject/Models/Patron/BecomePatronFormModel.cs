namespace GreenDoorProject.Models.Admin
{
    using GreenDoorProject.Data.Models;
    using System;
    
    public class BecomePatronFormModel
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public string  UserId { get; set; }
    }
}
