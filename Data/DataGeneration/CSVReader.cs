using System;
using System.IO;
using System.Collections.Generic;

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

    public class JSONGenerator {
        public JSONGenerator(){}

        public static List<string> convertToJSON(List<string[]> values){
            var jsonValues = new List<string>();

            for(int i = 1; i < values.Count; i++){
                string json = "{\n";

                json += $"\t\"cidade\": \"{values[i][(int)Colunas.Cidade]}\",\n";
                json += $"\t\"estado\": \"{values[i][(int)Colunas.Estado]}\",\n";
                json += $"\t\"populacao\": {values[i][(int)Colunas.Populacao]},\n";
                json += $"\t\"longitude\": {values[i][(int)Colunas.Longitude]},\n";
                json += $"\t\"latitude\": {values[i][(int)Colunas.Latitude]}\n";

                json += "}";

                jsonValues.Add(json);
            }

            return jsonValues;
        }
    }
}