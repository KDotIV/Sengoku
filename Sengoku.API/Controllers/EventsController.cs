using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Sengoku.API.Models;
using Sengoku.API.Models.Responses;
using Sengoku.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sengoku.API.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsController : Controller
    {
        private readonly EventsRepository _eventsRepository;

        public EventsController(EventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        [HttpGet]
        [Route("GetEvents")]
        public async Task<ActionResult> GetEventsAsync(int limit = 20, int page = 0,
            int sort = -1)
        {
            var events = await _eventsRepository.GetEvents(limit, page, sort);
            return Ok(events);
        }

        [HttpGet("GetEvent/id/{eventId}", Name = "GetEventById")]
        public async Task<ActionResult> GetEventById(string eventId)
        {
            var result = await _eventsRepository.GetEventById(eventId);
            if (result == null) return BadRequest(new ErrorResponse("No Event with that ID"));
            return Ok(result);
        }
        [HttpGet("GetEvent/name/{eventName}", Name = "GetEventByName")]
        public async Task<ActionResult> GetEventByName(string eventName)
        {
            var result = await _eventsRepository.GetEventByName(eventName);
            if (result == null) return BadRequest(new ErrorResponse("No Events with that Name"));
            return Ok(new EventsResponse(result));
        }
        [HttpGet("GetEvents/game/{gameName}", Name= "GetEventsByGame")]
        public async Task<ActionResult> GetEventsbyGame(string gameName, int limit = 20, int page = 0,
            int sort = -1)
        {
            var events = await _eventsRepository.GetEventsbyGame(gameName, limit, page, sort);
            if (events == null) return BadRequest(new ErrorResponse("No Events under that Game"));
            return Ok(new EventsResponse(events, page, null));
        }
    }
}
