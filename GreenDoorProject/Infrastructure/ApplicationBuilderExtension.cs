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

            //SeedGenres(data);

            //SeedHalls(data);

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
                new Genre { Name = "Romance" },
                new Genre { Name = "Mystery" },
                new Genre { Name = "Fantasy/SciFi" },
                new Genre { Name = "Thriller/Horror" },
                new Genre { Name = "Young adult" },
                new Genre { Name = "Children's Fiction" },
                new Genre { Name = "Inspirational/Self-help/Religious" },
                new Genre { Name = "Biography/Autobiography/Memoir" }
            });

            data.SaveChanges();
        }
    }
}
