using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Sengoku.API.Models.Responses
{
    public class PlotResponse
    {
        [JsonProperty("plot", NullValueHandling = NullValueHandling.Ignore)]
        public Plot Plot { get; set; }
        public bool Success { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public PlotResponse(bool success, string message, Plot plot = null)
        {
            Plot = plot;
            Success = success;
            if (success) SuccessMessage = message;
            else ErrorMessage = message;
        }
    }
}
