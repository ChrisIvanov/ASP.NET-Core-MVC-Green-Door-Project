namespace GreenDoorProject.Data
{
    using GreenDoorProject.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    
    public class GreenDoorProjectDbContext : IdentityDbContext
    {
        public GreenDoorProjectDbContext
            (DbContextOptions<GreenDoorProjectDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; init; }
        public DbSet<Game> Games { get; init; }
        public DbSet<Song> Songs { get; init; }
        public DbSet<Music> Music { get; init; }
        public DbSet<Actor> Actors { get; init; }
        public DbSet<Movie> Movies { get; init; }
        public DbSet<Author> Authors { get; init; }
        public DbSet<ActorMovie> ActorMovies { get; init; }
        public DbSet<Projection> Projections { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(b => b.AuthorBooks)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ActorMovie>()
                .HasKey(x => new { x.MovieId, x.ActorId });

            builder.Entity<ActorMovie>()
               .HasOne(x => x.Actor)
               .WithMany(x => x.ActorMovies)
               .HasForeignKey(x => x.ActorId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ActorMovie>()
               .HasOne(x => x.Movie)
               .WithMany(x => x.ActorMovies)
               .HasForeignKey(x => x.MovieId)
               .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
