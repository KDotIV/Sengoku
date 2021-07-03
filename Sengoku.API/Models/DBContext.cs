using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sengoku.API.Models
{
    public class DBContext : IMongoDBContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        public IClientSessionHandle Sesssion { get; set; }
        public DBContext(IOptions<MongoSettings> configuration)
        {
            _mongoClient = new MongoClient(configuration.Value.MongoURI);
            _db = _mongoClient.GetDatabase(configuration.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }
    }
}
