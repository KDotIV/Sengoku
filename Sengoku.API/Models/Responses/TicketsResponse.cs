using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Sengoku.API.Models.Responses
{
    public class TicketsResponse
    {
        [JsonProperty("product", NullValueHandling = NullValueHandling.Ignore)]
        public Tickets Ticket { get; set; }
        [JsonProperty("productList", NullValueHandling = NullValueHandling.Ignore)]
        public object TicketList { get; set; }
        public int Page { get; set; }
        public bool Success { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public TicketsResponse(bool success, string message, Tickets ticket = null)
        {
            Ticket = ticket;
            Success = success;
            if (success) SuccessMessage = message;
            else ErrorMessage = message;
        }
        public TicketsResponse(bool success, string message, int page, IReadOnlyList<Tickets> ticketList = null)
        {
            TicketList = ticketList;
            Success = success;
            if (success) SuccessMessage = message;
            else ErrorMessage = message;
            Page = page;
        }
    }
}
