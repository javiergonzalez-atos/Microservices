using System;
using System.Collections.Generic;
using System.Windows.Input;
using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace CommandsService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder appBuilder)
        {
            using (var serviceScoped = appBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScoped.ServiceProvider.GetRequiredService<IPlatformDataClient>();
                var platforms = grpcClient.ReturnALlPlatforms();

                var repository = serviceScoped.ServiceProvider.GetRequiredService<ICommandRepo>();
                
                SeedData(repository, platforms);

            }
        }

        private static void SeedData(ICommandRepo repository, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("Seeding new platforms");
            foreach (var platform in platforms)
            {
                if (!repository.ExternalPlatformExists(platform.ExternalID))
                {
                    repository.CreatePlatform(platform);
                }

                repository.SaveChanges();
            }
        }
    }
    
}