using System;
using MongoDB.Driver.GeoJsonObjectModel;

namespace DIO.Mongo_API.Data.Collections
{
    public class Cidade
    {
        public Cidade(string n, string e, int p, double lt, double lg){
            this.Nome = n;
            this.Estado = e;
            this.Populacao = p;
            this.Localizacao = new GeoJson2DGeographicCoordinates(lg, lt);
        }
       
        public string Nome {get; set;}
        public string Estado {get; set;}
        public int Populacao {get; set;}
        public GeoJson2DGeographicCoordinates Localizacao { get; set; }
    }
}