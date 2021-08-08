namespace GreenDoorProject.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.User;

    public class User : IdentityUser
    {
        [Required]
        [Display(Name = "Full Name")]
        [StringLength(FullNameMaxLength, MinimumLength = FullNameMinLength)]
        public string FullName { get; set; }
    }
}
