using System;
using System.IO;
using System.Collections.Generic;
using DIO.Mongo_API.Data.Collections;
using DIO.Mongo_API.Models;
using MongoDB.Driver;

/*
    Campos de cidades.csv:
        0 CITY
        1 STATE
        9 IBGE_POP
        23 LONG
        24 LAT
 */

namespace DIO.Mongo_API.Data.DataGeneration
{
    public enum Colunas : int {
        Cidade = 0,
        Estado = 1,
        Populacao = 9,
        Longitude = 23,
        Latitude = 24
    }
    public class CSVReader {

        public CSVReader(){}

        private static string getFilePath(){
            string fileName = "cidades.csv";
            var path = Path.GetFullPath(".");
            var filePath = $"{path}/Data/DataGeneration/{fileName}";

            return filePath;
        }

        public static List<string[]> readFromFile(){
            var filePath = getFilePath();

            var reader = new StreamReader(filePath);

            var valuesList = new List<string[]>();

            while(!reader.EndOfStream){
                var values = reader.ReadLine().Split(";");
                valuesList.Add(values);
            }

            return valuesList;
        }
    }
    public class ObjectGenerator {
        public ObjectGenerator(){}

        public static List<Cidade> getObjects(List<string[]> values){
            var cidades = new List<Cidade>();

            for(int i = 1; i < values.Count; i++){
                var cidade = new Cidade(values[i][(int)Colunas.Cidade], 
                                         values[i][(int)Colunas.Estado], 
                                         int.Parse(values[i][(int)Colunas.Populacao]), 
                                         Double.Parse(values[i][(int)Colunas.Longitude]), 
                                         Double.Parse(values[i][(int)Colunas.Latitude]), 0);

                cidades.Add(cidade);
            }

            return cidades;
        }
    }

    public class CollectionPopulator {
        IMongoCollection<Cidade> _cidadesCollection;

        public CollectionPopulator(){
            IMongoDatabase _db = Data.MongoDB.getMongoDBClient().GetDatabase("covidDB");
            _cidadesCollection = _db.GetCollection<Cidade>("cidade");
        }

        public void populate(){
           var cidades = ObjectGenerator.getObjects(CSVReader.readFromFile());

           _cidadesCollection.InsertMany(cidades);
        }
    }
}