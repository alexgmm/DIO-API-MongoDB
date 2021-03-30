using System;
using System.Collections.Generic;
using DIO.Mongo_API.Data.Collections;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace DIO.Mongo_API.Data.DataGeneration {
        /* Distribuição demográfica aproximada 
            < 10: 14%
           10-39: 50%
           40-59: 24%
           60-69: 6%
           70-89: 4%
           90-120: 2%
        */
    public enum FaixasEtarias {
        Ate10 = 14,
        Ate39 = 50,
        Ate59 = 24,
        Ate69 = 6,
        Ate89 = 4,
        Ate120 = 2
    }
    public class DateGenerator {
        private Random generator;
        private int baseAge;
        public DateGenerator(int baseAge){
            this.generator = new Random();
            this.baseAge = baseAge;
        }

        private bool isLeapYear(int year){
            return ((year % 4 ==0) && (year % 100 != 0)) || (year % 400 == 0);
        }

        private int getYear(){
            int currentYear = DateTime.Now.Year;
            int year = currentYear - baseAge;
            return year;
        }

        private int getRandomMonth(){
            return generator.Next(1,13);
        }

        private int getRandomDay(int year, int month){
            if(month == 2){
                if(isLeapYear(year))
                    return generator.Next(1,30);
                else
                    return generator.Next(1,29);
            } else if(month == 1 || month == 3 || month == 7 || month == 8 || month == 10 || month == 12){
                return generator.Next(1,32);
            } else {
                return generator.Next(1,31);
            }
        }
        public DateTime getRandomDate(){
            int year = getYear();
            int month = getRandomMonth();
            int day = getRandomDay(year, month);

            return DateTime.Parse($"{year}-{month}-{day}");
        }
    }
    public class InputCasesGenerator{
        private int population;
        private GeoJson2DGeographicCoordinates coordinates;
        public InputCasesGenerator(){}
        public InputCasesGenerator(Cidade c){
            this.population = c.Populacao;
            this.coordinates = c.Localizacao.Coordinates;
        }
        private int[] getCasesPerAgeGroup(int confirmedCases){
            int[] cases = new int[6];

            cases[0] = (int)FaixasEtarias.Ate10*confirmedCases/100;
            cases[1] = (int)FaixasEtarias.Ate39*confirmedCases/100;
            cases[2] = (int)FaixasEtarias.Ate59*confirmedCases/100;
            cases[3] = (int)FaixasEtarias.Ate69*confirmedCases/100;
            cases[4] = (int)FaixasEtarias.Ate89*confirmedCases/100;
            cases[5] = (int)FaixasEtarias.Ate120*confirmedCases/100;

            return cases;
        }
        private int getAgeForAgeGroup(FaixasEtarias faixa){
            var rand = new Random();

            switch (faixa)
            {
                case FaixasEtarias.Ate10:
                    return rand.Next(5, 11);
                case FaixasEtarias.Ate39:
                    return rand.Next(11, 40);
                case FaixasEtarias.Ate59:
                    return rand.Next(40, 60);
                case FaixasEtarias.Ate69:
                    return rand.Next(60, 70);
                case FaixasEtarias.Ate89:
                    return rand.Next(70, 90);
                case FaixasEtarias.Ate120:
                    return rand.Next(90, 121);
                default:
                    return 0;
            }
        }
        private int getNumberOfCasesForPopulation(){
            double factor;

            if(population < 50000)
                factor = 0.00005;
            else if(population < 200000)
                factor = 0.0001;
            else if(population < 500000)
                factor = 0.0003;
            else if(population < 1000000)
                factor = 0.0008;
            else if(population < 3000000)
                factor = 0.003;
            else if(population < 8000000)
                factor = 0.01;
            else 
                factor = 0.06;

            return (int)Math.Floor(factor * population);
        }
        private List<int> getAgesForPopulation(){
            var ages = new List<int>();

            var casesPerAgeGroup = getCasesPerAgeGroup(getNumberOfCasesForPopulation());

            for(int i=0; i<casesPerAgeGroup[0]; i++){
                int age = getAgeForAgeGroup(FaixasEtarias.Ate10);
                ages.Add(age);
            }

            for(int i=0; i<casesPerAgeGroup[0]; i++){
                int age = getAgeForAgeGroup(FaixasEtarias.Ate39);
                ages.Add(age);
            }

            for(int i=0; i<casesPerAgeGroup[0]; i++){
                int age = getAgeForAgeGroup(FaixasEtarias.Ate59);
                ages.Add(age);
            }

            for(int i=0; i<casesPerAgeGroup[0]; i++){
                int age = getAgeForAgeGroup(FaixasEtarias.Ate69);
                ages.Add(age);
            }

            for(int i=0; i<casesPerAgeGroup[0]; i++){
                int age = getAgeForAgeGroup(FaixasEtarias.Ate89);
                ages.Add(age);
            }

            for(int i=0; i<casesPerAgeGroup[0]; i++){
                int age = getAgeForAgeGroup(FaixasEtarias.Ate120);
                ages.Add(age);
            }

            return ages;
        }
        public List<Infectado> generateInputCases(){
            var inputs = new List<Infectado>();

            var ages = getAgesForPopulation();

            foreach(var a in ages){
                var dateGenerator = new DateGenerator(a);
                var birthDate = dateGenerator.getRandomDate();

                string sex = new Random().Next(0,2) == 0 ? "M" : "F";

                var input = new Infectado(birthDate, sex, coordinates);

                inputs.Add(input);
            }

            return inputs;
        }
    }

    public class InputCasesPopulator {
        public InputCasesPopulator(){}
        private static List<Cidade> GetCidades(){
            IMongoDatabase _db = Data.MongoDB.getMongoDBClient().GetDatabase("covidDB");

            IMongoCollection<Cidade> _cidadesCollection;

            _cidadesCollection = _db.GetCollection<Cidade>("cidade");

            return _cidadesCollection.Find(c => true).ToList<Cidade>();
        }
        private static IMongoCollection<Infectado> GetCollection(){
            IMongoDatabase _db = Data.MongoDB.getMongoDBClient().GetDatabase("covidDB");
            return _db.GetCollection<Infectado>("infectado");
        }
        private static void UpdateCount(Cidade cidade, int count){
            var filter = Builders<Cidade>.Filter.Eq("Localizacao", cidade.Localizacao);
            var update = Builders<Cidade>.Update.Set("Contagem", count);

            var cidadeColection = CollectionPopulator.GetCidadeCollection();
            cidadeColection.UpdateOne(filter, update);
        }
        public static void Populate(){
            var cidades = GetCidades();

            var Collection = GetCollection();

            foreach(var c in cidades){
                var inputCasesGenerator = new InputCasesGenerator(c);

                var cases = inputCasesGenerator.generateInputCases();
                
                if(cases.Count > 0){
                    Collection.InsertMany(cases);
                    UpdateCount(c, cases.Count);
                }
            }
        }
    }
}