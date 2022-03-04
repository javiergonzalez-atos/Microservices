using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using PlatformService.Models;
using System.Linq;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
                SeedData(dbContext, isProd);
            }
        }

        private static void SeedData(AppDbContext dbContext, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("--> Attempting to apply migrations");
                try
                {
                    dbContext.Database.Migrate();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"--> Could not run migration {e.Message}");
                }
            }
            
            if (!dbContext.Platforms.Any())
            {
                Console.WriteLine("--> Seeding data");
                dbContext.Platforms.AddRange(
     new Platform {Name = "Dot Net",Publisher = "Microsoft", Cost = "Free"}, 
                new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free"});

                dbContext.SaveChanges();
            }
            Console.WriteLine("--> We already have data");
        }
    }
}