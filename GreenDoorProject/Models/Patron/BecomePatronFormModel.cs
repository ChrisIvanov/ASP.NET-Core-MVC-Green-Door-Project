namespace GreenDoorProject.Models.Admin
{
    using System;
    
    public class BecomePatronFormModel
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public string  UserId { get; set; }

        public DateTime PatronSince { get; init; } = DateTime.UtcNow;
    }
}
