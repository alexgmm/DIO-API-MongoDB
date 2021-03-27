using System;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoDB.Bson;

namespace DIO.Mongo_API.Data.Collections
{
    public class Cidade
    {
        public Cidade(string n, string e, int p, double lt, double lg, int c){
            this.Nome = n;
            this.Estado = e;
            this.Populacao = p;
            this.Contagem = c;
            this.Localizacao = new GeoJson2DGeographicCoordinates(lg, lt);
        }
        public ObjectId Id { get; set; }
        public string Nome {get; set;}
        public string Estado {get; set;}
        public int Populacao {get; set;}
        public int Contagem {get; set;}
        public GeoJson2DGeographicCoordinates Localizacao { get; set; }

        public override string ToString()
        {
            return $"{Nome} - {Estado}";
        }
    }
}