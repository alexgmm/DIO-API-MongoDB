using System;
using MongoDB.Driver.GeoJsonObjectModel;

namespace DIO.Mongo_API.Data.Collections
{
    public class Infectado
    {
        public Infectado(DateTime dataNascimento, string sexo, double latitude, double longitude)
        {
            this.DataNascimento = dataNascimento;
            this.Sexo = sexo;
            this.Localizacao = GeoJson.Point(new GeoJson2DGeographicCoordinates(longitude, latitude));
        }

        public Infectado(DateTime dataNascimento, string sexo, GeoJson2DGeographicCoordinates localizacao)
        {
            this.DataNascimento = dataNascimento;
            this.Sexo = sexo;
            this.Localizacao = GeoJson.Point(localizacao);
        }
        
        public DateTime DataNascimento { get; set; }
        public string Sexo { get; set; }
        public GeoJsonPoint<GeoJson2DGeographicCoordinates> Localizacao { get; set; }
    }
}