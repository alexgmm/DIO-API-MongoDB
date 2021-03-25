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
            CreateHostBuilder(args).Build().Run();
            /* var c = CSVReader.readFromFile();
            var j = JSONGenerator.convertToJSON(c);

            Console.WriteLine(j[0]); */
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
