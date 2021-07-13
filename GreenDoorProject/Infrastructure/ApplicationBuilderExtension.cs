namespace GreenDoorProject.Infrastructure
{
    using System;
    using System.Linq;
    using GreenDoorProject.Data;
    using GreenDoorProject.Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices
                .ServiceProvider
                .GetService<GreenDoorProjectDbContext>();

            data.Database.Migrate();

            SeedGenres(data);

            SeedHalls(data);

            return app;
        }

        private static void SeedHalls(GreenDoorProjectDbContext data)
        {
            if (data.Halls.Any())
            {
                return;
            }

            data.Halls.AddRange(new[]
            {
                new Hall { Name = "Aurora" },
                new Hall { Name = "Imagina" },
                new Hall { Name = "FiveCircles" }
            });

            data.SaveChanges();
        }

        private static void SeedGenres(GreenDoorProjectDbContext data)
        {
            if (data.Genres.Any())
            {
                return;
            }

            data.Genres.AddRange(new[]
            {
                new Genre { Name = "Romance", Id = 1 },
                new Genre { Name = "Mystery", Id = 2 },
                new Genre { Name = "Fantasy/SciFi", Id = 3 },
                new Genre { Name = "Thriller/Horror", Id = 4 },
                new Genre { Name = "Young adult", Id = 5 },
                new Genre { Name = "Children's Fiction", Id = 6 },
                new Genre { Name = "Inspirational/Self-help/Religious", Id = 7 },
                new Genre { Name = "Biography/Autobiography/Memoir", Id = 8 }
            });

            data.SaveChanges();
        }
    }
}
