namespace GreenDoorProject.Data.Models
{
    using System.Collections.Generic;
    
    public class Genre
    {

        public int Id { get; init; }

        public string Name { get; set; }

        public IEnumerable<Book> Books { get; init; } = new List<Book>();     
    }
}
