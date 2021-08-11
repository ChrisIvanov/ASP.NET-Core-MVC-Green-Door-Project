namespace GreenDoorProject.Models.Member
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AddMemberFormModel
    {
        public string MembershipType { get; set; }
    
        [Display(Name = "Membership type")]
        public string MembershipId { get; set; }
        public IEnumerable<MembershipTypesViewModel> MembershipTypes { get; set; }
    }
}
