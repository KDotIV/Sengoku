using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Sengoku.API.Models.Responses
{
    public class UserResponse
    {
        [JsonProperty("info")] public User User { get; set; }
        [JsonProperty("auth_token")] public string AuthToken { get; set; }
        public bool Success { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public UserResponse(bool success, string message, User user = null)
        {
            User = user;
            Success = success;
            if (success) SuccessMessage = message;
            else ErrorMessage = message;
        }
    }
}
