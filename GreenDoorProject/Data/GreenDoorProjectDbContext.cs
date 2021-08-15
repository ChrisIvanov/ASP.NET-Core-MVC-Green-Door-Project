namespace GreenDoorProject.Data
{
    using GreenDoorProject.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class GreenDoorProjectDbContext : IdentityDbContext<Guest>
    {
        public GreenDoorProjectDbContext
            (DbContextOptions<GreenDoorProjectDbContext> options)
            : base(options)
        {
        }

        public DbSet<Actor> Actors { get; init; }
        public DbSet<ActorMovie> ActorMovies { get; init; }
        public DbSet<Author> Authors { get; init; }
        public DbSet<Book> Books { get; init; }
        public DbSet<Genre> Genres { get; init; }
        public DbSet<Member> Members { get; init; }
        public DbSet<Membership> Memberships { get; init; }
        public DbSet<Movie> Movies { get; init; }
        public DbSet<MusicAlbum> MusicAlbums { get; init; }
        public DbSet<Patron> Patrons { get; init; }
        public DbSet<Song> Songs { get; init; }

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

            builder.Entity<Book>()
                .HasOne(x => x.Genre)
                .WithMany(y => y.Books)
                .HasForeignKey(x => x.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Song>()
                .HasOne(x => x.MusicAlbum)
                .WithMany(y => y.Songs)
                .HasForeignKey(x => x.MusicAlbumId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Patron>()
                .HasOne<Guest>()
                .WithOne()
                .HasForeignKey<Patron>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Member>()
               .HasOne<Guest>()
               .WithOne()
               .HasForeignKey<Member>(m => m.Id)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Membership>()
                .Property(m => m.Price)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Patron>()
                .Property(p => p.Donations)
                .HasColumnType("decimal(18,2)");

            //builder.Entity<Admin>()
            //   .HasMany(a => a.Books)
            //   .WithOne()
            //   .HasForeignKey(a => a.AdminId)
            //   .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<Admin>()
            //   .HasMany(a => a.Games)
            //   .WithOne()
            //   .HasForeignKey(a => a.AdminId)
            //   .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<Admin>()
            //   .HasMany(a => a.Movies)
            //   .WithOne()
            //   .HasForeignKey(a => a.AdminId)
            //   .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<Admin>()
            //   .HasMany(a => a.Music)
            //   .WithOne()
            //   .HasForeignKey(a => a.AdminId)
            //   .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<Admin>()
            //   .HasMany(a => a.Projections)
            //   .WithOne()
            //   .HasForeignKey(a => a.AdminId)
            //   .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<Admin>()
            //   .HasMany(a => a.Songs)
            //   .WithOne()
            //   .HasForeignKey(a => a.AdminId)
            //   .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
