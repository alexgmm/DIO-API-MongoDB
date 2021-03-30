using DIO.Mongo_API.Data.Collections;
using DIO.Mongo_API.Data.DataGeneration;
using DIO.Mongo_API.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Linq;
using System;

namespace DIO.Mongo_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectadoController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Infectado> _infectadosCollection;

        public InfectadoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }

        private void ObterCidadeMaisProxima(Infectado infectado){
            var collection = CollectionPopulator.GetCidadeCollection();

            var point = infectado.Localizacao;
            int maxDistance = 1000000;

            IAsyncCursor<Cidade> cursor = collection.FindSync(new FilterDefinitionBuilder<Cidade>().Near(x => x.Localizacao, point, maxDistance: maxDistance));
            var hasNeighbors = cursor.Any();
            var cidades = cursor.ToList<Cidade>();
            foreach(var c in cidades) Console.WriteLine(c);
        }
        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            ObterCidadeMaisProxima(infectado);
                
            try {
                _infectadosCollection.InsertOne(infectado);
                return StatusCode(201, "Casp adicionado com sucesso");
            }catch{
                return StatusCode(500, "Erro ao adicionar dados");
            }
        }

        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();
            
            return Ok(infectados);
        }
    }
}
