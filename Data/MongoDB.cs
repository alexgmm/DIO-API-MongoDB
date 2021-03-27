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

        public MongoDB()
        {
            DB = getMongoDatabase();
            MapClasses();
        }
        
        public static IMongoDatabase getMongoDatabase(){
            return getMongoDBClient().GetDatabase("covidDB");
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

        public static MongoClient getMongoDBClient(){
            try
            {
                string connectionString = System.Environment.GetEnvironmentVariable("MONGO_URI");
                var settings = MongoClientSettings.FromConnectionString(connectionString);
                return new MongoClient(settings);
            }
            catch (Exception ex)
            {
                throw new MongoException("It was not possible to connect to MongoDB", ex);
            }
        }
    }
}