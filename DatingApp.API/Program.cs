using System;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DatingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
          var host =  CreateHostBuilder(args).Build();
          using(var scope = host.Services.CreateScope()) {
              var service = scope.ServiceProvider;
              try
              {
                  var context = service.GetRequiredService<AppDbContext>();
                  context.Database.Migrate();
                  Seed.SeedUser(context);
              }
              catch (Exception ex)
              {
                 var logger = service.GetRequiredService<ILogger<Program>>();
                 logger.LogError(ex, "An error during migration");
              }
          }

          host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
