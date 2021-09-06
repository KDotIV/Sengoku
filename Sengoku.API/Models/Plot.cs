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
    public class Plot
    {
        [JsonProperty("_id")]
        [BsonId] public ObjectId Id { get; set; }
        [BsonElement("plotId")] public string plotId { get; set; }
        [BsonElement("text")] public string text { get; set; }
        [BsonElement("image")] public string image { get; set; }
        [BsonElement("clipRef")] public string clipRef { get; set; }
    }
}
