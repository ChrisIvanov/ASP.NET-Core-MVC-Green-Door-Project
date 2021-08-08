namespace GreenDoorProject.Services.Books.Models
{    
    public class BookDetailsServiceModel
    {
        public string Id { get; set; }

        public string BookTitle { get; set; }

        public int Pages { get; set; }

        public string ImagePath { get; set; }

        public string Description { get; set; }

        public double Rating { get; set; }

        public string Genre { get; set; }

        public byte[] Content { get; set; }

        public string Author { get; set; }
    }
}
