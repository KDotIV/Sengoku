using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sengoku.API.Models
{
    public interface IMongoDBContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
