namespace GreenDoorProject.Data
{
    public class DatabaseConstants
    {
        internal const int UserFirstAndLastNameMinLength = 3;
        internal const int UserFirstAndLastNameMaxLength = 50;

        internal const int UsernameMinLength = 3;
        internal const int UsernameMaxLength = 20;

        internal const int UserPasswordMinLength = 6;
        internal const int UserPasswordMaxLength = 20;

        internal const string EmailRegexPattern = 
            @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";


    }
}
