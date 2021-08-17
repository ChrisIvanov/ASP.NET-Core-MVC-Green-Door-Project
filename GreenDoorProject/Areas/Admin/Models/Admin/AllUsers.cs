namespace GreenDoorProject.Areas.Admin.Models.Admin
{
    using System.Collections.Generic;
    
    public class AllUsers
    {
        public ICollection<UserDetails> Users { get; set; } = new List<UserDetails>();
    }
}
