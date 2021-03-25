using System;
using DIO.Mongo_API.Data.Collections;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace DIO.Mongo_API.Data
{
    public class MongoDB
    {
        public IMongoDatabase DB { get; }

        public MongoDB(IConfiguration configuration)
        {
            try
            {
                string connectionString = System.Environment.GetEnvironmentVariable("MONGO_URI");
                var settings = MongoClientSettings.FromConnectionString(connectionString);
                var client = new MongoClient(settings);
                DB = client.GetDatabase(configuration["NomeBanco"]);
                MapClasses();
            }
            catch (Exception ex)
            {
                throw new MongoException("It was not possible to connect to MongoDB", ex);
            }
        }

        private void MapClasses()
        {
            var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("camelCase", conventionPack, t => true);

            if (!BsonClassMap.IsClassMapRegistered(typeof(Infectado)))
            {
                BsonClassMap.RegisterClassMap<Infectado>(i =>
                {
                    i.AutoMap();
                    i.SetIgnoreExtraElements(true);
                });
            }
        }
    }
}