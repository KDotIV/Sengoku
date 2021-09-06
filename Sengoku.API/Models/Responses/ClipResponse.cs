using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Sengoku.API.Models.Responses
{
    public class ClipResponse
    {
        [JsonProperty("clip", NullValueHandling = NullValueHandling.Ignore)]
        public Clip Clip { get; set; }
        public bool Success { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public ClipResponse(bool success, string message, Clip clip = null)
        {
            Clip = clip;
            Success = success;
            if (success) SuccessMessage = message;
            else ErrorMessage = message;
        }
    }
}
