namespace GreenDoorProject.Services.Books.Models
{
    using GreenDoorProject.Data.Models;
    
    public class BookServiceModel
    {
        public string Id { get; set; }

        public string BookTitle { get; set; }

        public string AuthorFirstName { get; set; }

        public string AuthorLastName { get; set; }

        public int Pages { get; set; }

        public string ImagePath { get; set; }

        public Genre Genre { get; set; }

        public string Description { get; set; }

        public byte[] Contents { get; set; }

        public double Rating { get; set; }

        public byte[] Content { get; set; }
    }
}
