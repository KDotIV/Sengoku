using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sengoku.API.Models
{
    public class Address
    {
        [BsonElement("building")]
        public string Building { get; set; }
        [BsonElement("street")]
        public string Street { get; set; }
        [BsonElement("zipcode")]
        public string Zipcode { get; set; }
    }
}