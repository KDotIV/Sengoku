using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Sengoku.API.Models.Responses
{
    public class OrdersResponse
    {
        [JsonProperty("product", NullValueHandling = NullValueHandling.Ignore)]
        public Orders Order { get; set; }
        [JsonProperty("productList", NullValueHandling = NullValueHandling.Ignore)]
        public object OrderList { get; set; }
        public int Page { get; set; }
        public bool Success { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public OrdersResponse(bool success, string message, Orders order = null)
        {
            Order = order;
            Success = success;
            if (success) SuccessMessage = message;
            else ErrorMessage = message;
        }
        public OrdersResponse(bool success, string message, int page, IReadOnlyList<Orders> orderList = null)
        {
            OrderList = orderList;
            Success = success;
            if (success) SuccessMessage = message;
            else ErrorMessage = message;
            Page = page;
        }
    }
}
