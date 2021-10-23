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
    public class Legend
    {
        [JsonProperty("_id")]
        [BsonId] public ObjectId Id { get; set; }
        [BsonElement("legendId")] public string legendId { get; set; }
        [BsonElement("subject")] public string subject { get; set; }
        [BsonElement("summary")] public string summary { get; set; }
        [BsonElement("plotPoints")] public Plot[] plotPoints { get; set; }
        [BsonElement("game")] public string game { get; set; }
    }
}
