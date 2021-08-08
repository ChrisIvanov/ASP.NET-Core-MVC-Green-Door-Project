namespace GreenDoorProject.Services.Members
{
    using System.Linq;
    using GreenDoorProject.Data;

    public class MemberService : IMemberService
    {
        private readonly GreenDoorProjectDbContext data;

        public MemberService(GreenDoorProjectDbContext data) 
            => this.data = data;

        public bool IsMember(string userId)
            => this.data.Members
            .Any(m => m.UserId == userId);

        //add expiration date for the membership
        //and a notification for expiring membership
        //in the last week before expiration
    }
}
