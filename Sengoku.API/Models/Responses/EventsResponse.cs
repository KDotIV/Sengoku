using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Sengoku.API.Models.Responses
{
    public class EventsResponse
    {
        [JsonProperty("events", NullValueHandling = NullValueHandling.Ignore)]
        public Events Events { get; set; }
        [JsonProperty("eventsList", NullValueHandling = NullValueHandling.Ignore)]
        public object EventsList { get; set; }
        [JsonProperty("updatedType", NullValueHandling = NullValueHandling.Ignore)]
        public string UpdatedType { get; set; }
        public int Page { get; set; }
        public Dictionary<string, object> Filters { get; set; }

        public EventsResponse(Events events)
        {
            if(events != null)
            {
                Events = events;
                UpdatedType = (events.LastUpdated is DateTime) ? "Date" : "Other";
            }
        }
        public EventsResponse(IReadOnlyList<Events> eventsList, int page, Dictionary<string, object> filters)
        {
            EventsList = eventsList;
            Page = page;
            Filters = filters ?? new Dictionary<string, object>();
        }
    }
}
