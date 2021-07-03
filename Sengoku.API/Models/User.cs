using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sengoku.API.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId] public ObjectId Id { get; set; }
        [BsonElement("userid")] public string userId { get; set; }
        [BsonElement("name")] public string Name { get; set; }
        [BsonElement("password")] public string Password { get; set; }
        [BsonElement("email")] public string Email { get; set;  }
        [BsonElement("createdAt")] public DateTime CreatedAt { get; set; }
        [BsonElement("isBlocked")] public Boolean IsBlocked { get; set; }
        [BsonElement("lastupdated")] public DateTime LastUpdated { get; set; }
    }
}
