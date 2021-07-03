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
    public class Receipt
    {
        [JsonProperty("_id")]
        [BsonId] public ObjectId Id { get; set; }
        [BsonElement("receipt_id")] public string Receipt_Id { get; set; }
        [BsonElement("user_id")] public string User_Id { get; set; }
        [BsonElement("venue_fee")] public string Venue_Fee {get; set;} 
        [BsonElement("product")] public Products[] Product { get; set; }
        [BsonElement("processing_fee")] public string Processing_Fee { get; set; }
        [BsonElement("total_cost")] public string Total_Cost { get; set; }
        [BsonElement("payment_amount")] public string Payment_Amount { get; set; }
        [BsonElement("payment_date")] public DateTime Date { get; set; }
        [BsonElement("payment_method")] public string Payment_Method { get; set; }
        [BsonElement("transaction_id")] public string Transaction_Id { get; set; }
    }
}
