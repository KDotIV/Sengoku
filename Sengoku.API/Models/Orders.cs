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
    public class Orders
    {
        [JsonProperty("_id")]
        [BsonId] public ObjectId Id { get; set; }
        [BsonElement("order_id")] public string Order_Id { get; set; }
        [BsonElement("receipt")] public Receipt Receipt { get; set; }
        [BsonElement("shipping_address")] public Address Shipping_Address { get; set; }
        [BsonElement("billing_address")] public Address Billing_Address { get; set; }
        [BsonElement("date")] public DateTimeOffset OrderDate { get; set; }
        [BsonElement("completed_date")] public DateTimeOffset CompletedDate { get; set; }
    }
}
