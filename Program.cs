using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using DIO.Mongo_API.Data.DataGeneration;

namespace DIO.Mongo_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            var icp = new InputCasesPopulator(10000);
            icp.populate();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
