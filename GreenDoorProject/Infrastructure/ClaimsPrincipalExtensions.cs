namespace GreenDoorProject.Infrastructure
{
    using System.Security.Claims;

    using static Areas.Admin.AdminConstants;
    
    public static class ClaimsPrincipalExtensions
    {
        public static string GetId (this ClaimsPrincipal guest)
            => guest.FindFirst(ClaimTypes.NameIdentifier).Value;

        public static bool IsAdmin(this ClaimsPrincipal guest)
            => guest.IsInRole(AdministratorRoleName);
    }
}
