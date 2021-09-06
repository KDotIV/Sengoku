using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sengoku.API.Models
{
    [BsonIgnoreExtraElements]
    public class PlayerCards
    {
        [JsonProperty("_id")]
        [BsonId] public ObjectId Id { get; set; }
        [BsonElement("playerId")] public string playerId { get; set; }
        [BsonElement("name")] public string Name { get; set; }
        [BsonElement("events")] public Events[] events { get; set; }
        [BsonElement("image")] public string image { get; set; }
        [BsonElement("stats")] public Stats stats { get; set; }
        [BsonElement("social")] public string[] social { get; set; }
    }
}
