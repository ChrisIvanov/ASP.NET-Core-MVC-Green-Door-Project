namespace GreenDoorProject.Data
{
    public class DataConstants
    {
        //Book, Author, Game and Movie description constant
        internal const int DefaultDescriptionMaxLength = 1000;

        //Add User details validation constants
        internal const int UserFirstAndLastNameMinLength = 3;
        internal const int UserFirstAndLastNameMaxLength = 50;

        internal const int UsernameMinLength = 3;
        internal const int UsernameMaxLength = 20;

        internal const int UserPasswordMinLength = 6;
        internal const int UserPasswordMaxLength = 20;

        internal const string EmailRegexPattern =
            @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        //Add Book details validation constants
        internal const int BookTitleMinLength = 5;
        internal const int BookTitleMaxLength = 50;

        internal const int BookPagesMinLength = 5;
        internal const int BookPagesMaxLength = 2500;

        //Add Author details validation constants
        internal const int AuthorNameMinLength = 3;
        internal const int AuthorNameMaxLength = 50;

        //Add Movie details validation constants
        internal const int MovieTitleMinLength = 4;
        internal const int MovieTitleMaxLength = 20;

        internal const int MovieDirectorNameMinLength = 10;
        internal const int MovieDirectorNameMaxLength = 50;

        internal const int MovieYearOfReleaseMinValue = 1888;
        internal const int MovieYearOfReleaseMaxValue = 2021;

    }
}
