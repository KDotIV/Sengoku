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
    public class Products
    {
        [JsonProperty("_id")]
        [BsonId] public ObjectId Id { get; set; }
        [BsonElement("product_id")] public string Product_Id { get; set;  }
        [BsonElement("product_name")] public string Product_Name { get; set; }
        [BsonElement("description")] public string Description { get; set; }
        [BsonElement("price")] public decimal Price { get; set; }
        [BsonElement("stock")] public int Stock { get; set; }
    }
}
