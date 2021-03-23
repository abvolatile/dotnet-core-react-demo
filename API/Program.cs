using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; //for CreateScope()
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence; //for our DataContext

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run(); //default way
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<DataContext>(); //getting it as a service b/c we added it as a service in Startup.cs
                await context.Database.MigrateAsync(); //hover over Migrate to see what it will do!
                await Seed.SeedData(context);
            }
            catch (System.Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred during migration");
                throw;
            }

            await host.RunAsync(); //NOW we start our application (instead of right up at the top before we do any of the db stuff)
            //using the MigrateAsync and RunAsync isn't vitally necessary, but since we're using an async SeedData method, we had to change our Main function to be async (and return a Task instead of void) - so we might as well make these other things the async versions as well
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
