namespace GreenDoorProject.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Membership;

    public class Membership
    {
        public int Id { get; init; }

        [Required]
        [StringLength(MembershipNameMaxLength, MinimumLength = MembershipNameMinLength)]
        public string Name { get; set; }

        [Required]
        [Range(9.99, 89.99)]
        public decimal Price { get; set; }
    }
}
