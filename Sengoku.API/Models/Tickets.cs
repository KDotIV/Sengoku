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
        [BsonElement("user")] public string User_Id { get; set; }
        [BsonElement("payment_date")] public DateTime Payment_Date { get; set; }
        [BsonElement("venue_fee")] public decimal Venue_Fee { get; set; }
        [BsonElement("processing_fee")] public decimal Processing_Fee { get; set; }
        [BsonElement("total_cost")] public decimal Total_Cost { get; set; }
        [BsonElement("payment_amount")] public decimal Payment_Amount { get; set; }
        [BsonElement("payment_method")] public string Payment_Method { get; set; }
        [BsonElement("transaction_id")] public string Transaction_Id { get; set; }
    }
}
