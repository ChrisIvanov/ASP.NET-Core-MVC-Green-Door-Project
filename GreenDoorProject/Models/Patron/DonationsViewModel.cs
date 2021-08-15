namespace GreenDoorProject.Models.Patron
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Info;
    
    public class DonationsViewModel
    {
        [Required]
        [Display(Name = "Donation")]
        public decimal DonationAmount { get; set; }

        [StringLength(DefaultClassInfoMaxLength)]
        public string PersonalNote { get; set; }
    }
}
