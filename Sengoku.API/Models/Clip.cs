using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sengoku.API.Models
{
    [BsonIgnoreExtraElements]
    public class Clip
    {
        [BsonId] public ObjectId Id { get; set; }
        [BsonElement("clipId")] public string clipId { get; set; }
        [BsonElement("url")] public string url { get; set; }
        [BsonElement("name")] public string name { get; set; }
        [BsonElement("game")] public string game { get; set; }
    }
}
