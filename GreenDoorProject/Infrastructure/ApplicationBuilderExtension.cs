﻿namespace GreenDoorProject.Infrastructure
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using GreenDoorProject.Data;
    using GreenDoorProject.Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using static Areas.Admin.AdminConstants;

    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();
            var services = scopedServices.ServiceProvider;

            MigrateDatabase(services);

            SeedGenres(services);
            SeedMemberships(services);
            SeedAdministrator(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<GreenDoorProjectDbContext>();

            data.Database.Migrate();
        }

        private static void SeedMemberships(IServiceProvider services)
        {
            var data = services.GetRequiredService<GreenDoorProjectDbContext>();

            if (data.Memberships.Any())
            {
                return;
            }

            data.Memberships.AddRange(new[]
            {
                new Membership { Name = "One Month", Price = 9.99m },
                new Membership { Name = "Three Months", Price = 26.99m },
                new Membership { Name = "Six Months", Price = 50.99m },
                new Membership { Name = "Annual", Price = 89.99m }
            });

            data.SaveChanges();
        }

        private static void SeedGenres(IServiceProvider services)
        {
            var data = services.GetRequiredService<GreenDoorProjectDbContext>();

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

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<Guest>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdministratorRoleName };

                    await roleManager.CreateAsync(role);

                    const string adminEmail = "admin@gdp.com";
                    const string adminPassword = "admin12";

                    var guest = new Guest
                    {
                        Email = adminEmail,
                        UserName = adminEmail,
                        FullName = "Admin"
                    };

                    await userManager.CreateAsync(guest, adminPassword);

                    await userManager.AddToRoleAsync(guest, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
