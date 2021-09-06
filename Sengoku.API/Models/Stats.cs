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
    public class Stats
    {
        [JsonProperty("_id")]
        [BsonId] public ObjectId Id { get; set; }
        [BsonElement("statId")] public string statId { get; set; }
        [BsonElement("playstyle")] public string playstyle { get; set; }
        [BsonElement("organization")] public string organization { get; set; }
        [BsonElement("region")] public string region { get; set; }
        [BsonElement("graph_data")] public int[] graphData {get; set;}
        [BsonElement("characters")] public string[] characters { get; set; }
        [BsonElement("games")] public string[] games { get; set; }
    }
}
