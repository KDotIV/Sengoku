using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Sengoku.API.Models.Responses
{
    public class LegendResponse
    {
        [JsonProperty("legend", NullValueHandling = NullValueHandling.Ignore)]
        public Legend Legend { get; set; }
        public bool Success { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public LegendResponse(bool success, string message, Legend legends = null)
        {
            Legend = legends;
            Success = success;
            if (success) SuccessMessage = message;
            else ErrorMessage = message;
        }
    }
}
