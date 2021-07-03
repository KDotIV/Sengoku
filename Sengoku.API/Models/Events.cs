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
    public class Events
    {
        [JsonProperty("_id")]
        [BsonId] public ObjectId Id { get; set; }
        [BsonElement("address")] public Address Address { get; set; }
        [BsonElement("name")] public string Name { get; set; }
        [BsonElement("status")] public string Status { get; set; }
        [BsonElement("date")] public DateTime Date { get; set; }
        [BsonElement("lastupdated")] public DateTime LastUpdated { get; set; }
        [BsonElement("event_id")] public string Event_Id { get; set; }
        [BsonElement("city")] public string City { get; set; }
        [BsonElement("game")] public string Game { get; set; }
    }
}