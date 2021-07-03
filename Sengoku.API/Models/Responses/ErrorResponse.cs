using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sengoku.API.Models.Responses
{
    public class ErrorResponse
    {
        public string Error { get; set; }
        public ErrorResponse(string errorMessage)
        {
            Error = errorMessage;
        }
    }
}