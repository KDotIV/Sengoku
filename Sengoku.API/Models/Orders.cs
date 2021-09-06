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
        [BsonElement("user_id")] public User User { get; set; }
        [BsonElement("date")] public DateTime OrderDate { get; set; }
        [BsonElement("completed_date")] public DateTime CompletedDate { get; set; }
        [BsonElement("product")] public Products[] Products { get; set; }
        [BsonElement("processing_fee")] public decimal Processing_Fee { get; set; }
        [BsonElement("total_cost")] public decimal Total_Cost { get; set; }
        [BsonElement("payment_amount")] public decimal Payment_Amount { get; set; }
        [BsonElement("payment_method")] public string Payment_Method { get; set; }
        [BsonElement("transaction_id")] public string Transaction_Id { get; set; }


    }
}
