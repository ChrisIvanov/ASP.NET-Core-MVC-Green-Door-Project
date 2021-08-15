namespace GreenDoorProject.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;

    using static Data.DataConstants.Guest;

    public class Guest : IdentityUser
    {
        [Required]
        [Display(Name = "Full Name")]
        [StringLength(FullNameMaxLength, MinimumLength = FullNameMinLength)]
        public string FullName { get; set; }
    }
}