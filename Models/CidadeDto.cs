using System;

namespace DIO.Mongo_API.Models
{
    public class CidadeDto
    {
        public string Nome {get; set;}
        public string Estado {get; set;}
        public int Populacao {get; set;}
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}