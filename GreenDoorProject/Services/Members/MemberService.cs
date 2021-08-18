namespace GreenDoorProject.Services.Members
{
    using System;
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

        public bool MembershipExpiring(string userId)
        {
            var membership = this.data.Members
                .Where(u => u.UserId == userId)
                .FirstOrDefault();

            var membershipEnd = membership.MembershipEnd;
            var dateToday = DateTime.Now;

            if (membershipEnd.Subtract(dateToday).TotalDays < 7)
            {
                return true;
            }

            return false;
        }

        public void RemoveMember(string userId)
        {
            var member = this.data.Members.Find(userId);

            this.data.Members.Remove(member);
            this.data.SaveChanges();
        }
    }
}
