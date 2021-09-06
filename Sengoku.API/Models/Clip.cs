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
    public class Clip
    {
        [JsonProperty("_id")]
        [BsonId] public ObjectId Id { get; set; }
        [BsonElement("clipId")] public string clipId { get; set; }
        [BsonElement("url")] public string url { get; set; }
        [BsonElement("name")] public string name { get; set; }
        [BsonElement("game")] public string game { get; set; }
    }
}
