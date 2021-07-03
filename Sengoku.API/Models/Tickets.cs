using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Sengoku.API.Models
{
    [BsonIgnoreExtraElements]
    public class Tickets
    {
        [JsonProperty("_id")]
        [BsonId] public ObjectId Id { get; set; }
        [BsonElement("confirmation_id")] public string Confirmation_Id { get; set; }
        [BsonElement("pass_type")] public string Pass_Type { get; set; }
        [BsonElement("event")] public Events Event { get; set; }
        [BsonElement("receipt")] public Receipt receipt { get; set; }
        [BsonElement("user_d")] public string User_Id { get; set; }
    }
}
