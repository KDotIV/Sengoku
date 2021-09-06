using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Sengoku.API.Models.Responses
{
    public class PlayerCardResponse
    {
        [JsonProperty("playercard", NullValueHandling = NullValueHandling.Ignore)]
        public PlayerCards playerCard { get; set; }
        public bool Success { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public PlayerCardResponse(bool success, string message, PlayerCards playerCards = null)
        {
            playerCard = playerCards;
            Success = success;
            if (success) SuccessMessage = message;
            else ErrorMessage = message;
        }
    }
}
