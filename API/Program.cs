using System;
using API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            var host= CreateHostBuilder(args).Build();
            using var scope=host.Services.CreateScope();//here usong is equivalent to finally
            var context= scope.ServiceProvider.GetRequiredService<StoreContext>();
            var logger=scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            try{ 
                context.Database.Migrate();
                DbInitializer.Intialize(context);
            }
            catch(Exception ex){
                logger.LogError(ex,"Problem in migrating data");
            }
            host.Run();
            // finally{
            //     scope.Dispose();
            // }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
