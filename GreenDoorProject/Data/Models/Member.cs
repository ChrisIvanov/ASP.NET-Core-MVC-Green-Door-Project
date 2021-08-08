namespace GreenDoorProject.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Member
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string UserId { get; init; }

        [Required]
        public int MembershipId { get; set; }
        public Membership Membership { get; set; }

        public DateTime MembershipStart { get; set; } = DateTime.UtcNow;

        public DateTime MembershipEnd { get; set; }
    }
}
