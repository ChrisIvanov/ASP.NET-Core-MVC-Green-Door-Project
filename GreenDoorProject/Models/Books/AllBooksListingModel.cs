namespace GreenDoorProject.Models.Books
{   
    using GreenDoorProject.Data.Models;
    
    public class AllBooksListingModel
    {
        public int Id { get; set; }

        public string BookTitle { get; set; }

        public int Pages { get; set; }

        public string ImagePath { get; set; }

        public Genre Genre { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string AuthorName { get; set; }

        public double Recomendations { get; set; }
    }
}
