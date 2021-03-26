using System;

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
    public class InputCasesPopulator{
        private int population;
        public InputCasesPopulator(){}
        public InputCasesPopulator(int p){
            population = p;
        }

        public int[] getCasesPerAgeGroup(int confirmedCases){
            int[] cases = new int[6];

            cases[0] = (int)FaixasEtarias.Ate10*confirmedCases/100;
            cases[1] = (int)FaixasEtarias.Ate39*confirmedCases/100;
            cases[2] = (int)FaixasEtarias.Ate59*confirmedCases/100;
            cases[3] = (int)FaixasEtarias.Ate69*confirmedCases/100;
            cases[4] = (int)FaixasEtarias.Ate89*confirmedCases/100;
            cases[5] = (int)FaixasEtarias.Ate120*confirmedCases/100;

            return cases;
        }
        private int getNumberOfCasesForPopulation(){
            double a = 2.841817E-9, e = 1.232042;

            int number = (int)Math.Floor(a*population*Math.Pow(population, e));

            return number;
        }

        public void populate(){
            //var cidades = ObjectGenerator.getObjects(CSVReader.readFromFile());

            var cases = getCasesPerAgeGroup(getNumberOfCasesForPopulation());
            foreach(var c in cases) Console.WriteLine(c);
        }
    }
}