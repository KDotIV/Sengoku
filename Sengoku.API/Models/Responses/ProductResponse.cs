using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Sengoku.API.Models.Responses
{
    public class ProductResponse
    {
        [JsonProperty("product", NullValueHandling = NullValueHandling.Ignore)]
        public Products Product { get; set; }
        [JsonProperty("productList", NullValueHandling = NullValueHandling.Ignore)]
        public object ProductList { get; set; }
        public int Page { get; set; }
        public bool Success { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public ProductResponse(bool success, string message, Products product = null)
        {
            Product = product;
            Success = success;
            if (success) SuccessMessage = message;
            else ErrorMessage = message;
        }
        public ProductResponse(bool success, string message, int page, IReadOnlyList<Products> productList = null)
        {
            ProductList = productList;
            Success = success;
            if (success) SuccessMessage = message;
            else ErrorMessage = message;
            Page = page;
        }
    }
}
