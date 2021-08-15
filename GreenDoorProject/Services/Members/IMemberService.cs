namespace GreenDoorProject.Services.Members
{
    public interface IMemberService
    {
        bool IsMember(string userId);

        bool MembershipExpiring(string userId);

        void RemoveMember(string userId);
    }
}
