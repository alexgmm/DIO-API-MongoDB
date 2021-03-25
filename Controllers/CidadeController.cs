using DIO.Mongo_API.Data.Collections;
using DIO.Mongo_API.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DIO.Mongo_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CidadeController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Cidade> _cidadesCollection;

        public CidadeController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _cidadesCollection = _mongoDB.DB.GetCollection<Cidade>(typeof(Cidade).Name.ToLower());
        }

        [HttpPost]
        public ActionResult SalvarCidade([FromBody] CidadeDto dto)
        {
            var Cidade = new Cidade(dto.Nome, dto.Estado, dto.Populacao, dto.Latitude, dto.Longitude, 0);

            _cidadesCollection.InsertOne(Cidade);
            
            return StatusCode(201, "Cidade adicionada com sucesso");
        }

        [HttpGet]
        public ActionResult ObterCidades()
        {
            var Cidades = _cidadesCollection.Find(Builders<Cidade>.Filter.Empty).ToList();
            
            return Ok(Cidades);
        }
    }
}
