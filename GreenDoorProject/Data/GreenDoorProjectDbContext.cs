
namespace GreenDoorProject.Data
{
    using GreenDoorProject.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    
    public class GreenDoorProjectDbContext : IdentityDbContext
    {
        public GreenDoorProjectDbContext(DbContextOptions<GreenDoorProjectDbContext> options)
            : base(options)
        {
        }

        public DbSet<Actor> Actors { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Music> Music { get; set; }
        public DbSet<Projection> Projections { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
