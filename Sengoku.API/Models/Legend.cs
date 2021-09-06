using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sengoku.API.Models
{
    [BsonIgnoreExtraElements]
    public class Legend
    {
        [BsonId] public ObjectId Id { get; set; }
        [BsonElement("legendId")] public string legendId { get; set; }
        [BsonElement("subject")] public string subject { get; set; }
        [BsonElement("summary")] public string summary { get; set; }
        [BsonElement("plotpoints")] public Plot[] plotPoints { get; set; }
        [BsonElement("game")] public string game { get; set; }
    }
}
