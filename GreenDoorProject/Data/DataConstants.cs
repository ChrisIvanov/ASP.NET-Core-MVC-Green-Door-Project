namespace GreenDoorProject.Data
{
    public class DataConstants
    {
        internal class Actor
        {
            internal const int NameMinLength = 3;
            internal const int NameMaxLength = 50;

            internal const int YearOfBirthMinValue = 1900;
            internal const int YearOfBirthMaxValue = 2020;
        }

        internal class Author
        {
            internal const int NameMinLength = 3;
            internal const int NameMaxLength = 50;
        }

        internal class Book
        {
            internal const int TitleMinLength = 5;
            internal const int TitleMaxLength = 50;

            internal const int PagesMinLength = 5;
            internal const int PagesMaxLength = 2500;
        }

        internal class Info
        {
            internal const int DefaultClassInfoMaxLength = 1500;
        }

        internal class Game
        {
            internal const int NameMinLength = 3;
            internal const int NameMaxLength = 50;

            internal const int GameGanreNameMinLength = 5;
            internal const int GameGanreNameMaxLength = 40;

            internal const double PriceMinValue = 40.00;
            internal const double PriceMaxValue = 250.00;
        }

        internal class Ganre
        {
            internal const int GanreNameMinLength = 5;
            internal const int GanreNameMaxLength = 50;
        }

        internal class Membership
        {
            internal const int MembershipNameMinLength = 3;
            internal const int MembershipNameMaxLength = 15;
        }

        internal class Movie
        {
            internal const int TitleMinLength = 4;
            internal const int TitleMaxLength = 20;

            internal const int DirectorNameMinLength = 10;
            internal const int DirectorNameMaxLength = 50;

            internal const int YearOfReleaseMinValue = 1888;
            internal const int YearOfReleaseMaxValue = 2021;

            internal const double TicketPriceMinValue = 5.00;
            internal const double TicketPriceMaxValue = 25.00;
        }

        internal class Music
        {
            internal const int ArtistNameMinLength = 3;
            internal const int ArtistNameMaxLength = 30;

            internal const int AlbumTitleMinLength = 3;
            internal const int AlbumTitleMaxLength = 30;

            internal const double AlbumPriceMinValue = 10.00;
            internal const double AlbumPriceMaxValue = 150.00;

            internal const int SongNameMinLength = 3;
            internal const int SongNameMaxLength = 30;
        }

        internal class Song
        {
            internal const int SongNameMinLength = 5;
            internal const int SongNameMaxLength = 20;
        }

        internal class Guest
        {
            internal const int FullNameMinLength = 3;
            internal const int FullNameMaxLength = 50;

            internal const int GuestnameMinLength = 3;
            internal const int GuestnameMaxLength = 20;

            internal const int PasswordMinLength = 6;
            internal const int PasswordMaxLength = 20;

            internal const string EmailRegexPattern =
                @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        }
    }
}
